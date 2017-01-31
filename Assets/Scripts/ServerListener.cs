using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections.Generic;


public class ServerListener : MonoBehaviour
{

    public bool isconnection;
    Quaternion initializingRotation;
    private bool bordcreated = true;

    public GameObject wall;
    public GameObject stone;
    public GameObject water;
    public GameObject health;
    public GameObject coin;
    public GameObject tank;

    /*public GameObject P0 = null;
    public GameObject P1 = null;
    public GameObject P2 = null;
    public GameObject P3 = null;
    public GameObject P4 = null;
    public GameObject P5 = null;*/

    private List<Vector3> wallList = new List<Vector3>();
    private List<Vector3> stoneList = new List<Vector3>();
    private List<Vector3> waterList = new List<Vector3>();

    List<CoinObject> coinsToDraw = new List<CoinObject>();
    List<HealthObject> healthToDraw = new List<HealthObject>();

    //List<GameObject> toDestroy = new List<GameObject>();
    Queue<object> toInstanciate = new Queue<object>();

    // Use this for initialization
    void Start()
    {
        initializingRotation = transform.rotation;
        var thread = new System.Threading.Thread(listen);
        thread.Start();
    }

    // Update is called once per frame
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

        /*while (toInstanciate.Count > 0)
        {
            GameObject t = toInstanciate[0];
            toInstanciate.RemoveAt(0);
            GameObject game = Instantiate(health, c.getPosition(), initializingRotation) as GameObject;


        }*/

        

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

            if (list[0].ToString()=="P0")
            {
                GameObject clone = (GameObject)Instantiate(tank,pPosition, rotation);
                Destroy(clone, 1.23f);

                /*if (P0 != null)
                {
                    //DestroyImmediate(P0, true);
                    P0.GetComponent<Renderer>().enabled = false;
                }
                P0.GetComponent<Renderer>().enabled = true;
                P0 = Instantiate(tank, pPosition, rotation) as GameObject;*/
            }

            if (list[0].ToString() == "P1")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);

                /*if (P1 != null)
                {
                    //DestroyImmediate(P1,true);
                    P1.GetComponent<Renderer>().enabled = false;
                }
                P1.GetComponent<Renderer>().enabled = true;
                P1 = Instantiate(tank, pPosition, rotation) as GameObject;*/
            }

            if (list[0].ToString() == "P2")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);

                /*if (P2 != null)
                {
                    //DestroyImmediate(P2, true);
                    P2.GetComponent<Renderer>().enabled = false;
                }
                P2.GetComponent<Renderer>().enabled = true;
                P2 = Instantiate(tank, pPosition, rotation) as GameObject;*/
            }

            if (list[0].ToString() == "P3")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);
                /*if (P3 != null)
                {
                    //DestroyImmediate(P3, true);
                    P3.GetComponent<Renderer>().enabled = false;
                }
                P3.GetComponent<Renderer>().enabled = true;
                P3 = Instantiate(tank, pPosition, rotation) as GameObject;*/
            }

            if (list[0].ToString() == "P4")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);

                /*if (P4 != null)
                {
                    //DestroyImmediate(P4, true);
                    P4.GetComponent<Renderer>().enabled = false;
                }
                P4.GetComponent<Renderer>().enabled = true;
                P4 = Instantiate(tank, pPosition, rotation) as GameObject;*/
            }

            if (list[0].ToString() == "P5")
            {
                GameObject clone = (GameObject)Instantiate(tank, pPosition, rotation);
                Destroy(clone, 1.25f);

                /*if (P5 != null)
                {
                    //DestroyImmediate(P5, true);
                    P5.GetComponent<Renderer>().enabled = false;
                }
                P5.GetComponent<Renderer>().enabled = true;
                P5 = Instantiate(tank, pPosition, rotation) as GameObject;*/
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
                            /*String[] cod = pdata[1].Split(',');

                            Vector3 pPosition = new Vector3(Int32.Parse(cod[0]), -Int32.Parse(cod[1]));
                            Quaternion rotation = Quaternion.Euler(0, 0, 0);

                            int pDirection = Int32.Parse(pdata[2]);
                            if (pDirection == 0)
                            {
                                rotation = Quaternion.Euler(0, 0, 0);
                            }
                            if (pDirection == 1)
                            {
                                rotation = Quaternion.Euler(0, 0, 90);
                            }
                            if (pDirection == 2)
                            {
                                rotation = Quaternion.Euler(0, 0, 180);
                            }
                            if (pDirection == 3)
                            {
                                rotation = Quaternion.Euler(0, 0, -90);
                            }*/

                            String pShot = pdata[3];
                            String pHealth = pdata[4];
                            String pCoins = pdata[5];
                            String pPoints = pdata[6];

                            List<string> temp = new List<string>();
                            temp.Add(pdata[0]);
                            temp.Add(pdata[1]);
                            temp.Add(pdata[2]);

                            toInstanciate.Enqueue(temp);

                            /*if (pdata[0].Equals("P0"))
                            {
                                if (P0 != null)
                                {
                                    //Destroy(P0);
                                    toDestroy.Add(P0);
                                }
                                //P0 = Instantiate(tank, pPosition, rotation) as GameObject;
                                toInstanciate.Add(temp);
                            }
                            if (pdata[0].Equals("P1"))
                            {
                                if (P1 != null)
                                {
                                    //Destroy(P1);
                                    toDestroy.Add(P1);
                                }
                                //P1 = Instantiate(tank, pPosition, rotation) as GameObject;
                                toInstanciate.Add(temp);
                            }
                            if (pdata[0].Equals("P2"))
                            {
                                if (P2 != null)
                                {
                                    //Destroy(P2);
                                    toDestroy.Add(P2);
                                }
                                //P2 = Instantiate(tank, pPosition, rotation) as GameObject
                                toInstanciate.Add(temp);
                            }
                            if (pdata[0].Equals("P3"))
                            {
                                if (P3 != null)
                                {
                                    //Destroy(P3);
                                    toDestroy.Add(P3);
                                }
                                //P3 = Instantiate(tank, pPosition, rotation) as GameObject;
                                toInstanciate.Add(temp);
                            }
                            if (pdata[0].Equals("P4"))
                            {
                                if (P4 != null)
                                {
                                    //Destroy(P4
                                    toDestroy.Add(P4);
                                }
                                //P4 = Instantiate(tank, pPosition, rotation) as GameObject;
                                toInstanciate.Add(temp);
                            }
                            if (pdata[0].Equals("P5"))
                            {
                                if (P5 != null)
                                {
                                    //Destroy(P5);
                                    toDestroy.Add(P5);
                                }
                                //P5 = Instantiate(tank, pPosition, rotation) as GameObject;
                                toInstanciate.Add(temp);
                            }*/
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

    //inner class for coin
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

    //inner class for health
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

