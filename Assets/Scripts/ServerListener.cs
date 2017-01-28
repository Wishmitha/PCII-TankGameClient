using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;


public class ServerListener : MonoBehaviour
{

    public bool isconnection;
    public GameObject wall;
    public GameObject stone;
    public GameObject water;
    public GameObject health;
    public GameObject coin;
    Quaternion originalRot;
    private System.Collections.Generic.List<Vector3> bricks = new System.Collections.Generic.List<Vector3>();
    private System.Collections.Generic.List<Vector3> stones = new System.Collections.Generic.List<Vector3>();
    private System.Collections.Generic.List<Vector3> waters = new System.Collections.Generic.List<Vector3>();
    private bool bordInitialized = true;

    bool b, c = true;

    void Start()
    {

        originalRot = transform.rotation;
        UnityEngine.Debug.logger.Log("Thread start");
        var thread = new System.Threading.Thread(listen);
        thread.Start();

    }
    void Update()
    {

        if (!bordInitialized)
        {
            foreach (Vector3 vec in bricks)
            {
                Instantiate(wall, vec, originalRot);
            }
            foreach (Vector3 vec in stones)
            {
                Instantiate(stone, vec, originalRot);
            }
            foreach (Vector3 vec in waters)
            {
                Instantiate(water, vec, originalRot);
            }
            bordInitialized = true;
        }
    }
    void listen()
    {
        try
        {
            UnityEngine.Debug.logger.Log("Thread started");

            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
            listener.Start();

            while (true)
            {
 
                string value;

 
                using (var networkStream = listener.AcceptTcpClient().GetStream())
                {
 

                    var bytes = new System.Collections.Generic.List<byte>();

                    int asw = 0;
                    while (asw != -1)
                    {
                        asw = networkStream.ReadByte();
                        bytes.Add((Byte)asw);
                    }
                    if (b && c)
                    {

                        b = false;
                    }

                    value = Encoding.UTF8.GetString(bytes.ToArray());

                }
                String[] datas = value.Split(':');
                UnityEngine.Debug.logger.Log(datas[0] + "   " + value);
                if (datas[0].Equals("I"))
                {
                    UnityEngine.Debug.logger.Log("S" + datas[2]);
                    String walls = datas[2];
                    bricks = getVecotors(walls);
                    UnityEngine.Debug.logger.Log("S" + datas[3]);
                    String stone = datas[3];
                    stones = getVecotors(stone);
                    UnityEngine.Debug.logger.Log("P" + datas[4]);
                    String water = datas[4].Trim();
                    water = water.Substring(0, water.Length - 2);
                    waters = getVecotors(water);
                    UnityEngine.Debug.logger.Log("D");
                    bordInitialized = false;

                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception");
        }
    }
    System.Collections.Generic.List<Vector3> getVecotors(String data)
    {
        String[] cod = data.Split(';');
        System.Collections.Generic.List<Vector3> vectors = new System.Collections.Generic.List<Vector3>();

        foreach (String e in cod)
        {
            String[] d = e.Split(',');
            Vector3 v = new Vector3(Int32.Parse(d[0]), -Int32.Parse(d[1]), 0);
            vectors.Add(v);
        }
        return vectors;

    }


}

