
using UnityEngine;
using System.Collections;

public class AttackArea : MonoBehaviour {
	CharacterStatus status;
	
	void Start()
	{
		status = transform.root.GetComponent<CharacterStatus>();
	}
	
	
	public class AttackInfo
	{
		public int attackPower; // この攻撃の攻撃力.
		public Transform attacker; // 攻撃者.
	}
	
	
	// 攻撃情報を取得する.
	AttackInfo GetAttackInfo()
	{			
		AttackInfo attackInfo = new AttackInfo();
		// 攻撃力の計算.
		attackInfo.attackPower = status.Power;
        // 攻撃強化中
        if (status.powerBoost)
            attackInfo.attackPower += attackInfo.attackPower;

        attackInfo.attacker = transform.root;
		
		return attackInfo;
	}
	
	// 当たった.
	void OnTriggerEnter(Collider other)
	{
		// 攻撃が当たった相手のDamageメッセージをおくる.
		if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Base"))
		other.SendMessage("Damage",GetAttackInfo());
		// 攻撃した対象を保存.
		status.lastAttackTarget = other.transform.root.gameObject;
	}
	
	
	// 攻撃判定を有効にする.
	void OnAttack()
	{
		collider.enabled = true;
	}
	
	
	// 攻撃判定を無効にする.
	void OnAttackTermination()
	{
		collider.enabled = false;
	}
}
