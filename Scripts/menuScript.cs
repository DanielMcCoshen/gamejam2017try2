using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {

    public AudioSource a1;
    public AudioSource a2;
    
    public string[] levs;
    private string level;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onLevelSelect(string lev)
    {
        a1.Play();
        level = lev;
        Invoke("changeLevel", 0.5f);
    }

    private void changeLevel() {
        SceneManager.LoadScene(level);
    }
    public void onLevelRandom()
    {
        a1.Play();
        level = levs[Random.Range(0, levs.Length)];
        Invoke("changeLevel", 0.5f);
    }

    public void onExit()
    {
        Application.Quit();
    }

    public void onHighlight() {
        a2.Play();
    }
}

