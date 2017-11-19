using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleScript : MonoBehaviour {

    public float speed = 10;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().velocity = Random.onUnitSphere * speed;
        StartCoroutine("deathCount");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator deathCount()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

}
