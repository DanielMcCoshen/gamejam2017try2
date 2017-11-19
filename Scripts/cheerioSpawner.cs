using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cheerioSpawner : MonoBehaviour {

    public GameObject cheerio;

	// Use this for initialization
	void Start () {
        StartCoroutine("spawnio");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator spawnio()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.03f);
            Destroy(Instantiate(cheerio, new Vector3(Random.Range(transform.position.x - 5, transform.position.x + 5), transform.position.y, Random.Range(transform.position.z - 5, transform.position.z + 5)), Random.rotation), 10f);
        }
    }
}
