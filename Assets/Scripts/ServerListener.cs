using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Net;



public class ServerListener : MonoBehaviour
{

    public bool isconnection;

    Quaternion initializingRotation;
    /*
    Server messages are recived in a thread. but in unity can't draw images as soon as we get data.
    Those images should be created when and only when update method is called.
    Hence we sore server message's data in below. And later in update they are created
    */
    //Initializing details
    private System.Collections.Generic.List<Vector3> walles = new System.Collections.Generic.List<Vector3>();
    private System.Collections.Generic.List<Vector3> stones = new System.Collections.Generic.List<Vector3>();
    private System.Collections.Generic.List<Vector3> waters = new System.Collections.Generic.List<Vector3>();
    private bool bordInitialized = true;
    //Coin emerging details

    System.Collections.Generic.List<CoinObject> coinsToDraw = new System.Collections.Generic.List<CoinObject>();


    //Health emerging details

    System.Collections.Generic.List<HealthObject> healthToDraw = new System.Collections.Generic.List<HealthObject>();

    //Prefabs that need for the scinario
    public GameObject wall;
    public GameObject stone;
    public GameObject water;
    public GameObject health;
    public GameObject coin;

    //added by me
    /*public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject player5;*/


    // Use this for initialization
    void Start()
    {
        //Console.WriteLine("Thread is comming on the way");
        initializingRotation = transform.rotation;
        //UnityEngine.Debug.logger.Log("Thread is comming on the way");
        var thread = new System.Threading.Thread(listen);
        thread.Start();

    }
    void Update()
    {
        //initialize bord if data had recevied
        if (!bordInitialized)
        {
            foreach (Vector3 vec in walles)
            {
                Instantiate(wall, vec, initializingRotation);
            }
            foreach (Vector3 vec in stones)
            {
                Instantiate(stone, vec, initializingRotation);
            }
            foreach (Vector3 vec in waters)
            {
                Instantiate(water, vec, initializingRotation);
            }
            bordInitialized = true;
        }
        while (coinsToDraw.Count > 0)
        {
            CoinObject c = coinsToDraw[0];
            coinsToDraw.RemoveAt(0);
            GameObject game = Instantiate(coin, c.getPosition(), initializingRotation) as GameObject;
            //Send data to coin. So that it can work independently later
            game.SendMessage("setValues", new int[] { c.getTimeLeft(), c.getCoinValue() });
            UnityEngine.Debug.logger.Log("Coin   " + c.getX() + "," + c.getY() + " " + c.getCoinValue() + "  time" + c.getTimeLeft());
        }
        while (healthToDraw.Count > 0)
        {
            HealthObject c = healthToDraw[0];
            healthToDraw.RemoveAt(0);
            GameObject game = Instantiate(health, c.getPosition(), initializingRotation) as GameObject;
            //Send data to coin. So that it can work independently later
            game.SendMessage("setValues", c.getTimeLeft());
            UnityEngine.Debug.logger.Log("Health   " + c.getX() + "," + c.getY()  + "  time" + c.getTimeLeft());
        }
        
    }
   
    void listen()
    {
        try
        {
            //UnityEngine.Debug.logger.Log("Thread started");
            // Create the listener providing the IP Address and the port to listen on.
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
            listener.Start();

            // Listen in an endless loop. 
            while (true)
            {
                // We will store the textual representation of the bytes we receive. 
                string value;

                // Accept the sender and take message as a stream. 
                using (var networkStream = listener.AcceptTcpClient().GetStream())
                {
                    // Create a memory stream to copy the message. 

                    var bytes = new System.Collections.Generic.List<byte>();

                    int asw = 0;
                    while (asw != -1)
                    {
                        asw = networkStream.ReadByte();
                        bytes.Add((Byte)asw);
                    }

                    // Convert bytes to text. 
                    value = Encoding.UTF8.GetString(bytes.ToArray());

                }
                String[] datas = value.Split(':');
                //UnityEngine.Debug.logger.Log(datas[0]+"   "+value);
                if (datas[0].Equals("I"))
                {

                    String walls = datas[2];
                    walles = getVecotors(walls);
                    String stone = datas[3];
                    stones = getVecotors(stone);
                    String water = datas[4].Trim();
                    //At the end String there are unwanter  characters. one is # and other is a unicode
                    water = water.Substring(0, water.Length - 2);
                    waters = getVecotors(water);
                    bordInitialized = false;


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
                    //UnityEngine.Debug.logger.Log("Coin   " + coinPosition.x+","+coinPosition.y+" "+coinValue+"  time"+coinCountTime);

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
                    //UnityEngine.Debug.logger.Log("Health   " + healthPosition.x + "," + healthPosition.y + " " + healthCountTime);
                }
                // Call an external function (void) given. 

                else if (datas[0].ToUpper().Equals("P2"))
                {
                    String[] cod = datas[1].Split(',');
                    Vector3 healthPosition = new Vector3(Int32.Parse(cod[0]), -Int32.Parse(cod[1]));
                    datas[2] = datas[2].Trim();
                    datas[2] = datas[2].Substring(0, datas[2].Length - 2);
                    int healthCountTime = Int32.Parse(datas[2]);
                    HealthObject he = new HealthObject(healthPosition, healthCountTime);
                    healthToDraw.Add(he);
                    //UnityEngine.Debug.logger.Log("Health   " + healthPosition.x + "," + healthPosition.y + " " + healthCountTime);
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occured");
        }
    }
    /*
    A set off cordinates (2,3:2,4:....) will be passed to this method as a string.
    This method will decode them and create list of vectors fromt those cordinates..
    */
    System.Collections.Generic.List<Vector3> getVecotors(String data)
    {
        String[] cod = data.Split(';');
        System.Collections.Generic.List<Vector3> vectors = new System.Collections.Generic.List<Vector3>();

        foreach (String e in cod)
        {
            String[] d = e.Split(',');
            //Y is made minus because our grid system in unity is creates as such.
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
        public CoinObject(Vector3 pos, int coin, int time)
        {
            this.position = pos;
            this.coinValue = coin;
            this.timeLeft = time;
        }
    }
    class HealthObject
    {
        Vector3 position;
        int timeLeft;
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
        public HealthObject(Vector3 pos, int time)
        {
            this.position = pos;
            this.timeLeft = time;
        }
    }


}

 