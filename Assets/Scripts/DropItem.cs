using UnityEngine;
using System.Collections;

public class DropItem : MonoBehaviour {
	public enum ItemKind
	{
		Attack,
		Heal,
	};
	public ItemKind kind;
	
	void OnTriggerEnter(Collider other)
	{	
		// Playerか判定
		if( other.tag == "Player" ){
			// アイテム取得
			CharacterStatus aStatus = other.GetComponent<CharacterStatus>();
			aStatus.GetItem(kind);
			// 取得したらアイテムを消す
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		Vector3 velocity = Random.insideUnitSphere * 2.0f + Vector3.up * 8.0f;
		rigidbody.velocity = velocity;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
