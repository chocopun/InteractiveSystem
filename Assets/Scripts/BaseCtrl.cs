using UnityEngine;
using System.Collections;

public class BaseCtrl : MonoBehaviour {
    public float hp = 500.0f;
    GameObject gameManager;

    void Start () {
        gameManager = GameObject.Find("GameManager");
    }

    void Update () {
    }

    void Damage(AttackArea.AttackInfo attackInfo){
        hp -= attackInfo.attackPower;
        if (hp <= 0) {
            hp = 0;
            gameManager.SendMessage("EndGame");
        }
    }
}
