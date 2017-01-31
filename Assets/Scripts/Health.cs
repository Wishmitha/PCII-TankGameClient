using UnityEngine;

public class Health : MonoBehaviour {

    float timeLeft;
    bool b = true;
    void Start () {
	
	}
	
	void Update () {
        if (!b)
        
        {
            timeLeft -= Time.deltaTime * 1000;
            if (timeLeft < 0)
            {
                Debug.logger.Log("Health  vanished "  + "  time" + Time.time);
                Destroy(gameObject);
            }
        }
    }
    public void setValues(int timeLeft)
    {
        this.timeLeft = timeLeft;
        b = false;
        Debug.logger.Log("HValues set"+Time.time);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Tank")
        {
            Destroy(gameObject);
        }
    }
}
