using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameController:MonoBehaviour {
    public GameObject player1prefab;
    public GameObject player2prefab;
    public GameObject player3prefab;
    public GameObject player4prefab;

    public GameObject player1spawn;
    public GameObject player2spawn;
    public GameObject player3spawn;
    public GameObject player4spawn;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    public GameObject[] powerups;
    public GameObject[] powerUpSpawns;

    public float minPowerUpInterval;
    public float MaxPowerUpInterval;
    public float powerUpTimeOut;

    private int team1wins;
    private int team2wins;

    public GameObject winspamer;
    public GameObject team1winDrop;
    public GameObject team2winDrop;

    public Text team1Label;
    public Text team2Label;

    private bool reset = false;

    // Use this for initialization
    void Start() {
        team1wins = 0;
        team2wins = 0;
        startGame();
        StartCoroutine("spawnPowerUp");
    }

    void startGame() {
        reset = false;
        player1 = Instantiate(player1prefab, player1spawn.transform.position, player1spawn.transform.rotation);
        player2 = Instantiate(player2prefab, player2spawn.transform.position, player2spawn.transform.rotation);
        player3 = Instantiate(player3prefab, player3spawn.transform.position, player3spawn.transform.rotation);
        player4 = Instantiate(player4prefab, player4spawn.transform.position, player4spawn.transform.rotation);

        player1.GetComponent<Player>().opponent1 = player3;
        player1.GetComponent<Player>().opponent2 = player4;

        player2.GetComponent<Player>().opponent1 = player3;
        player2.GetComponent<Player>().opponent2 = player4;

        player3.GetComponent<Player>().opponent1 = player1;
        player3.GetComponent<Player>().opponent2 = player2;

        player4.GetComponent<Player>().opponent1 = player1;
        player4.GetComponent<Player>().opponent2 = player2;

        team1Label.text = team1wins + "/3";
        team2Label.text = team2wins + "/3";
    }

    // Update is called once per frame
    void Update() {
        if(player1 == null && player2 == null && !reset) {
            team2wins++;
            StartCoroutine("roundEnd");
            reset = true;
        }
        else if(player3 == null && player4 == null && !reset) {
            team1wins++;
            StartCoroutine("roundEnd");
            reset = true;
        }

        if (Input.GetAxis("P1BackButton") == 1)
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    private IEnumerator roundEnd() {
        if(team1wins >= 3) {
             StartCoroutine("gameEnd", 1);
        }
        else if(team2wins >= 3) {
            StartCoroutine("gameEnd", 2);
        }
        else {
            yield return new WaitForSeconds(2f);
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
                Destroy(g);
               
            }
            startGame();
        }
    }


    private IEnumerator spawnPowerUp() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(minPowerUpInterval, MaxPowerUpInterval));
            GameObject p = powerups[Random.Range(0, powerups.Length)];
            GameObject sp = powerUpSpawns[Random.Range(0, powerUpSpawns.Length)];
            Destroy(Instantiate(p, sp.transform.position, sp.transform.rotation), powerUpTimeOut);

        }
    }

    private IEnumerator gameEnd(int team) {

        if(team == 1) {
            winspamer.GetComponent<cheerioSpawner>().cheerio = team1winDrop;
        }
        else if(team == 2){
            winspamer.GetComponent<cheerioSpawner>().cheerio = team2winDrop;
        }
        winspamer.SetActive(true);
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
