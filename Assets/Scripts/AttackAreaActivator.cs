using UnityEngine;
using System.Collections;

public class AttackAreaActivator : MonoBehaviour {
	Collider[] attackAreaColliders; // 攻撃判定コライダの配列.
	
	void Start () {
		// 子供のGameObjectからAttackAreaスクリプトがついているGameObjectを探す。
		AttackArea[] attackAreas = GetComponentsInChildren<AttackArea>();
		attackAreaColliders = new Collider[attackAreas.Length];
		
		for (int attackAreaCnt = 0; attackAreaCnt < attackAreas.Length; attackAreaCnt++) {
			// AttackAreaスクリプトがついているGameObjectのコライダを配列に格納する.
			attackAreaColliders[attackAreaCnt] = attackAreas[attackAreaCnt].collider;
			attackAreaColliders[attackAreaCnt].enabled = false;  // 初期はfalseにしておく.
		}
	}
	
	// アニメーションイベントのStartAttackHitを受け取ってコライダを有効にする
	void StartAttackHit()
	{
		foreach (Collider attackAreaCollider in attackAreaColliders)
			attackAreaCollider.enabled = true;
	}
	
	// アニメーションイベントのEndAttackHitを受け取ってコライダを無効にする
	void EndAttackHit()
	{
		foreach (Collider attackAreaCollider in attackAreaColliders)
			attackAreaCollider.enabled = false;
	}
}
