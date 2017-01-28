using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

public class ServerConnect : MonoBehaviour
{
    private const int portNum = 6000;
    private const string hostName = "127.0.0.1";
    private const string message = "JOIN#";
    //public static 
    // Use this for initialization
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
        }
        catch (Exception ex)
        {

            System.Diagnostics.Debug.WriteLine(ex);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
