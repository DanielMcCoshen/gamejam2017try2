using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

    public float range;

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.SendMessage("applyDamage", 1);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
            if(Vector3.Distance(transform.position, g.transform.position) > range) {
                SendMessage("applyDamage", 4);
            }
        }
        Destroy(gameObject);
    }
}
