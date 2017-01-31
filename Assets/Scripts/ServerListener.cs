using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections.Generic;

using WindowsInput;


public class ServerListener : MonoBehaviour
{

    public bool isconnection;
    Quaternion initializingRotation;
    private bool bordcreated = true;

    //ServerConnect sc = new ServerConnect();

    public GameObject wall;
    public GameObject stone;
    public GameObject water;
    public GameObject health;
    public GameObject coin;
    public GameObject tank;

    public GameObject player;

    private List<Vector3> wallList = new List<Vector3>();
    private List<Vector3> stoneList = new List<Vector3>();
    private List<Vector3> waterList = new List<Vector3>();

    List<CoinObject> coinsToDraw = new List<CoinObject>();
    List<HealthObject> healthToDraw = new List<HealthObject>();

    Queue<object> toInstanciate = new Queue<object>();

    bool isInstanciate = false;

    void Start()
    {
        initializingRotation = transform.rotation;
        var thread = new System.Threading.Thread(listen);
        thread.Start();
    }

    void Update()
    {

        if (!bordcreated)
        {
            foreach (Vector3 vec in wallList)
            {
                Instantiate(wall, vec, initializingRotation);
            }
            foreach (Vector3 vec in stoneList)
            {
                Instantiate(stone, vec, initializingRotation);
            }
            foreach (Vector3 vec in waterList)
            {
                Instantiate(water, vec, initializingRotation);
            }

            bordcreated = true;
        }

        while (coinsToDraw.Count > 0)
        {
            CoinObject c = coinsToDraw[0];
            coinsToDraw.RemoveAt(0);
            GameObject game = Instantiate(coin, c.getPosition(), initializingRotation) as GameObject;
            game.SendMessage("setValues", new int[] { c.getTimeLeft(), c.getCoinValue() });
            UnityEngine.Debug.logger.Log("Coin   " + c.getX() + "," + c.getY() + " " + c.getCoinValue() + "  time" + c.getTimeLeft());
        }

        while (healthToDraw.Count > 0)
        {
            HealthObject c = healthToDraw[0];
            healthToDraw.RemoveAt(0);
            GameObject game = Instantiate(health, c.getPosition(), initializingRotation) as GameObject;
            game.SendMessage("setValues", c.getTimeLeft());
            UnityEngine.Debug.logger.Log("Health   " + c.getX() + "," + c.getY() + "  time" + c.getTimeLeft());
        }

        while(toInstanciate.Count>0)
        {

            //toInstanciate.RemoveAt(0);
            List<string> list = toInstanciate.Dequeue() as List<string>;
            

            String[] cod = list[1].ToString().Split(',');
            Vector3 pPosition = new Vector3(Int32.Parse(cod[0]), -Int32.Parse(cod[1]));

            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            int pDirection = Int32.Parse(list[2].ToString());
            if (pDirection == 0)
            {
                rotation = Quaternion.Euler(0, 0, 0);
            }
            if (pDirection == 1)
            {
                rotation = Quaternion.Euler(0, 0, -90);
            }
            if (pDirection == 2)
            {
                rotation = Quaternion.Euler(0, 0, 180);
            }
            if (pDirection == 3)
            {
                rotation = Quaternion.Euler(0, 0, 90);
            }

            /*if (list[0].ToString()=="P0")
            {
                if (!isInstanciate)
                {
                    GameObject clone = (GameObject)Instantiate(player, pPosition, rotation);
                    //Destroy(clone, 1.25f);
                    isInstanciate = true;
                }
            }*/

            if (list[0].ToString() == "P0")
            {
                 GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                 Destroy(clone, 1.25f);
            }

            if (list[0].ToString() == "P1")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);
            }

            if (list[0].ToString() == "P2")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);
            }

            if (list[0].ToString() == "P3")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);
            }

            if (list[0].ToString() == "P4")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);
            }

            if (list[0].ToString() == "P5")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);
            }

        }

    }

    void listen()
    {
        try
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
            listener.Start();

            while (true)
            {
                string value;

                using (var networkStream = listener.AcceptTcpClient().GetStream())
                {
                    var bytes = new List<byte>();

                    int asw = 0;
                    while (asw != -1)
                    {
                        asw = networkStream.ReadByte();
                        bytes.Add((Byte)asw);
                    }

                    value = Encoding.UTF8.GetString(bytes.ToArray());

                }
                String[] datas = value.Split(':');
                if (datas[0].Equals("I"))
                {

                    String walls = datas[2];
                    wallList = getVecotors(walls);
                    String stone = datas[3];
                    stoneList = getVecotors(stone);
                    String water = datas[4].Trim();
                    water = water.Substring(0, water.Length - 2);
                    waterList = getVecotors(water);
                    bordcreated = false;
                }
                else if (datas[0].ToUpper().Equals("C"))
                {
                    String[] cod = datas[1].Split(',');
                    Vector3 coinPosition = new Vector3(Int32.Parse(cod[0]), -Int32.Parse(cod[1]));
                    int coinCountTime = Int32.Parse(datas[2]);
                    datas[3] = datas[3].Trim();
                    datas[3] = datas[3].Substring(0, datas[3].Length - 2);
                    int coinValue = Int32.Parse(datas[3]);
                    CoinObject o = new CoinObject(coinPosition, coinValue, coinCountTime);
                    coinsToDraw.Add(o);

                }
                else if (datas[0].ToUpper().Equals("L"))
                {
                    String[] cod = datas[1].Split(',');
                    Vector3 healthPosition = new Vector3(Int32.Parse(cod[0]), -Int32.Parse(cod[1]));
                    datas[2] = datas[2].Trim();
                    datas[2] = datas[2].Substring(0, datas[2].Length - 2);
                    int healthCountTime = Int32.Parse(datas[2]);
                    HealthObject he = new HealthObject(healthPosition, healthCountTime);
                    healthToDraw.Add(he);
                }

                else if (datas[0].ToUpper().Equals("G"))
                {
                    var tanks = datas;

                    for (int i = 1; i < tanks.Length - 1; i++)
                    {
                        String[] pdata = tanks[i].Split(';');
                        if (true)
                        {
                            String pShot = pdata[3];
                            String pHealth = pdata[4];
                            String pCoins = pdata[5];
                            String pPoints = pdata[6];

                            List<string> temp = new List<string>();
                            temp.Add(pdata[0]);
                            temp.Add(pdata[1]);
                            temp.Add(pdata[2]);

                            toInstanciate.Enqueue(temp);
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occured");
        }
    }
    List<Vector3> getVecotors(String data)
    {
        String[] cod = data.Split(';');
        List<Vector3> vectors = new List<Vector3>();

        foreach (String e in cod)
        {
            String[] d = e.Split(',');
            Vector3 v = new Vector3(Int32.Parse(d[0]), -Int32.Parse(d[1]), 0);
            vectors.Add(v);
        }
        return vectors;

    }

    class CoinObject
    {
        Vector3 position;
        int coinValue;
        int timeLeft;

        public CoinObject(Vector3 pos, int coin, int time)
        {
            this.position = pos;
            this.coinValue = coin;
            this.timeLeft = time;
        }
        public float getX()
        {
            return position.x;
        }
        public float getY()
        {
            return position.y;
        }
        public int getCoinValue()
        {
            return coinValue;
        }
        public int getTimeLeft()
        {
            return timeLeft;
        }
        public Vector3 getPosition()
        {
            return position;
        }

    }

    class HealthObject
    {
        Vector3 position;
        int timeLeft;
        public HealthObject(Vector3 pos, int time)
        {
            this.position = pos;
            this.timeLeft = time;
        }
        public float getX()
        {
            return position.x;
        }
        public float getY()
        {
            return position.y;
        }
        public Vector3 getPosition()
        {
            return position;
        }
        public int getTimeLeft()
        {
            return timeLeft;
        }

    }
}

