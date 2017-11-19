using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {


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
    public void onLevelRandom(string[] levs)
    {
        SceneManager.LoadScene(levs[Random.Range(0, levs.Length)]);
    }

    public void onExit()
    {
        Application.Quit();
    }
}

