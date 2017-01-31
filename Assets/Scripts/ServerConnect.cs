using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class ServerConnect : MonoBehaviour
{
    private const int portNum = 6000;
    private const string hostName = "127.0.0.1";
    private const string message = "JOIN#";
    public static readonly String Shoot = "SHOOT#";
    public static readonly String Up = "UP#";
    public static readonly String Down = "DOWN#";
    public static readonly String Left = "LEFT#";
    public static readonly String Right = "RIGHT#";
    public int x=1;

    void Start()
    {
        try
        {
            using (var client = new TcpClient(hostName, portNum))
            {
                var byteData = Encoding.ASCII.GetBytes(message);
                client.GetStream().Write(byteData, 0, byteData.Length);
            }
            Console.WriteLine("Connected");

            InvokeRepeating("move", 0, 1);

            /*string[] dir = {"right","left","up","down","shoot" };
            InvokeRepeating(dir[x], 0, 1);*/
        }
        catch (Exception ex)
        {

            System.Diagnostics.Debug.WriteLine(ex);
        }

    }

    void Update()
    {
        System.Random rnd = new System.Random();
        x = rnd.Next(0, 10000) % 10;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("up is called");
            new Thread(up).Start();


        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            new Thread(down).Start();

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            new Thread(left).Start();

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            new Thread(right).Start();

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            new Thread(shoot).Start();
        }

    }
    public  void up()
    {
        send(Up);
    }
    public  void down()
    {
        send(Down);
    }
    public  void left()
    {
        send(Left);
    }
    public  void right()
    {
        send(Right);
    }
    public  void shoot()
    {
        send(Shoot);
    }

    void send(String Message)
    {
        try
        {
            using (var client = new TcpClient(hostName, portNum))
            {
                var byteData = Encoding.ASCII.GetBytes(Message);
                client.GetStream().Write(byteData, 0, byteData.Length);
            }
            Console.WriteLine("Data send");
        }
        catch (Exception ex)
        {

            System.Diagnostics.Debug.WriteLine(ex);
        }
    }

    public void move()
    {
        int y = x;

        if (y == 0 || y==3 || y==5 || y==7)
        {
            right();

        }else if (y == 1)
        {
            left();
        }else if (y==2)
        {
            up();
        }else if (y == 4 || y==6)
        {
            down();
        }
        else
        {
            shoot();
        }
    }
}
