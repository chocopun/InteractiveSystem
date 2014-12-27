using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject wargPrefab;
    public GameObject dragonPrefab;
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;
    public static List<GameObject> existPlayers;
    public static List<GameObject> existEnemys;
    GameObject player, warg;

    void Start () {
        existPlayers = new List<GameObject>();
        existEnemys = new List<GameObject>();
    }

    void SpawnPlayer () {
        existPlayers.Add(Instantiate(playerPrefab,playerSpawnPoint.position,playerSpawnPoint.rotation) as GameObject);
    }

    void SpawnWarg () {
        existEnemys.Add(Instantiate(wargPrefab,enemySpawnPoint.position,enemySpawnPoint.rotation) as GameObject);
    }

    void SpawnDragon () {
        existEnemys.Add(Instantiate(dragonPrefab,enemySpawnPoint.position,enemySpawnPoint.rotation) as GameObject);
    }
}
