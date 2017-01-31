﻿using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    // Use this for initialization
    int value;
    float timeLeft;
    bool b = true;
    
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!b)
        {

            timeLeft -= Time.deltaTime*1000;
            if (timeLeft < 0)
            {
                UnityEngine.Debug.logger.Log("Coin  vanished " + value + "  time" + timeLeft+" "+Time.time);
                Destroy(gameObject);
            }
        }
        
	}

    public void setValues(int[] data)
    {
        this.timeLeft = data[0];
        this.value = data[1];
        b = false;
        UnityEngine.Debug.logger.Log("Coin  start " + value + "  time" + timeLeft + " " + Time.time);
        //Debug.logger.Log("Values set"+timeLeft);

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.logger.Log("colided");
        if (col.gameObject.tag == "Tank")
        {
            //col.gameObject.SendMessage("coinAdded" ,value);
            Destroy(gameObject);
            Debug.logger.Log("Destroyed");
        }
    }
}
