using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

    public float range;
    public float boompower; 

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.SendMessage("applyDamage", 1);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
            if(Vector3.Distance(transform.position, g.transform.position) < range) {
                g.SendMessage("applyDamage", 4);
                g.GetComponent<Rigidbody>().AddExplosionForce(boompower, transform.position, range*2);
            }
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enviromental")) {
            if(Vector3.Distance(transform.position, g.transform.position) < range) {
                g.GetComponent<Rigidbody>().AddExplosionForce(boompower, transform.position, range * 2);
            }
        }
        Destroy(gameObject);
    }
}
