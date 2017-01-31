using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    int value;
    float timeLeft;
    bool b = true;
    
	void Start () {
        
    }
	
	void Update () {
        if (!b)
        {

            timeLeft -= Time.deltaTime*1000;
            if (timeLeft < 0)
            {
                Debug.logger.Log("Coin  vanished " + value + "  time" + timeLeft+" "+Time.time);
                Destroy(gameObject);
            }
        }
        
	}

    public void setValues(int[] data)
    {
        this.timeLeft = data[0];
        this.value = data[1];
        b = false;
        Debug.logger.Log("Coin  start " + value + "  time" + timeLeft + " " + Time.time);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.logger.Log("colided");
        if (col.gameObject.tag == "Tank")
        {
            Destroy(gameObject);
            Debug.logger.Log("Destroyed");
        }
    }
}
