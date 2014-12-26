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
    GameObject playerBase, enemyBase;
    PlayerCtrl playerCtrl;
    EnemyCtrl enemyCtrl;

    void Start () {
        existPlayers = new List<GameObject>();
        existEnemys = new List<GameObject>();
        playerBase = GameObject.Find("PlayerBase");
        enemyBase = GameObject.Find("EnemyBase");
        playerCtrl = GetComponent<PlayerCtrl>();
        enemyCtrl = GetComponent<EnemyCtrl>();
    }

    void SpawnPlayer () {
        existPlayers.Add(Instantiate(playerPrefab,playerSpawnPoint.position,playerSpawnPoint.rotation) as GameObject);
        //player = Instantiate(playerPrefab,playerSpawnPoint.position,playerSpawnPoint.rotation) as GameObject;
    }

    void SpawnWarg () {
        existEnemys.Add(Instantiate(wargPrefab,enemySpawnPoint.position,enemySpawnPoint.rotation) as GameObject);
        //warg = Instantiate(wargPrefab,enemySpawnPoint.position,enemySpawnPoint.rotation) as GameObject;
    }

    void SpawnDragon () {
        existEnemys.Add(Instantiate(dragonPrefab,enemySpawnPoint.position,enemySpawnPoint.rotation) as GameObject);
    }

    public static GameObject PlayerAttackTarget() {
        return existPlayers[0];
    }

    public static GameObject EnemyAttackTarget() {
        return existEnemys[0];
    }
}
