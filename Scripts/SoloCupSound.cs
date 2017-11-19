using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloCupSound : MonoBehaviour {

    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    public float threashold;

    private AudioSource aud;
	// Use this for initialization
	void Start () {
        aud = GetComponent<AudioSource>();
        switch(Random.Range(0, 3)) {
            case 0:
                aud.clip = one;
                break;
            case 1:
                aud.clip = two;
                break;
            case 2:
                aud.clip = three;
                break;
        }
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude > threashold) {
            aud.Play();
        }
    }
}
