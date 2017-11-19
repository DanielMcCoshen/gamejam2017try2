using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {

    public string[] levs;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onLevelSelect(string lev)
    {
        SceneManager.LoadScene(lev);
    }
    public void onLevelRandom()
    {
        SceneManager.LoadScene(levs[Random.Range(0, levs.Length)]);
    }

    public void onExit()
    {
        Application.Quit();
    }
}

