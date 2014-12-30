using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public GameObject spawnManager;

    void Awake() {
        InvokeRepeating("SendSpawnWarg", 2.0f, 2.0f);
    }

    void Start () {
        spawnManager = GameObject.Find("SpawnManager");
    }

    void Update () {

    }

    void SendSpawnWarg(){
        spawnManager.SendMessage("SpawnWarg");
    }

    void EndGame () {
        Application.LoadLevel("Home");
    }
}
