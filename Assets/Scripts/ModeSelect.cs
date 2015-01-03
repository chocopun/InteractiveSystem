using UnityEngine;
using System.Collections;

public class ModeSelect : MonoBehaviour {
    public static string mode = "";

    public void OnEasyClick() {
        mode = "easy";
        Application.LoadLevel("GameScene");
    }

    public void OnHardClick() {
        mode = "hard";
        Application.LoadLevel("GameScene");
    }
}
