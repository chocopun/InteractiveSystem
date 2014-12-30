using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckInputField : MonoBehaviour {

    InputField inputField;
    MonoSQLiteConnect sqlite;
	// Use this for initialization
	void Start () {
	   inputField = GetComponent<InputField>();
       sqlite = GetComponent<MonoSQLiteConnect>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("return"))
        sqlite.SendMessage("RecieveInputField", inputField.text);
	}
}
