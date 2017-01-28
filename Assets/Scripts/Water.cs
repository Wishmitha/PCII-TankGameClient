using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    string type = "water";

    int num_water = 10;

    public GameObject water;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < num_water; i++)
        {
            float fx = Random.Range(-5, 5);
            float fy = Random.Range(-5, 5);

            int x = (int)fx;
            int y = (int)fy;

            Vector2 pos = new Vector2(x + 0.5f, y + 0.5f);
            Instantiate(water, pos, transform.rotation);
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
