using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public int powerMode = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.SendMessage("changeMode", powerMode);
            Destroy(gameObject);
        }
    }
}
