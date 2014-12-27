using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
	CharacterStatus status;
	CharaAnimation charaAnimation;
	Transform attackTarget;
	public float attackRange = 1.5f;
	GameObject enemyAttackTarget;
	GameObject enemyBase;
	private GameObject nearbyObj;
	private float targetUpdateTime = 0;
	
	// ステートの種類.
	enum State {
		Walking,
		Attacking,
		Died,
	} ;
	
	State state = State.Walking;		// 現在のステート.
	State nextState = State.Walking;	// 次のステート.
	
	
	// Use this for initialization
	void Start () {
		status = GetComponent<CharacterStatus>();
		charaAnimation = GetComponent<CharaAnimation>();
		enemyBase = GameObject.Find("EnemyBase");
		nearbyObj = GetNearbyObj(gameObject, "Enemy");
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case State.Walking:
			Walking();
			break;
		case State.Attacking:
			Attacking();
			break;
		}
		
		if (state != nextState)
		{
			state = nextState;
			switch (state) {
			case State.Walking:
				WalkStart();
				break;
			case State.Attacking:
				AttackStart();
				break;
			case State.Died:
				Died();
				break;
			}
		}

		targetUpdateTime += Time.deltaTime;
		if (targetUpdateTime >= 1.0f) {
		nearbyObj = GetNearbyObj(gameObject, "Enemy");
		if(nearbyObj)
			attackTarget = nearbyObj.transform;
		targetUpdateTime = 0;
		}
	}
	
	
	// ステートを変更する.
	void ChangeState(State nextState)
	{
		this.nextState = nextState;
	}
	
	void WalkStart()
	{
		StateStartCommon();
	}
	
	void Walking()
	{
		if (attackTarget) {
			Vector3 hitPoint = attackTarget.position;
			hitPoint.y = transform.position.y;
			float distance = Vector3.Distance(hitPoint,transform.position);
			if (distance < attackRange) {
				ChangeState(State.Attacking);
			} else {
				SendMessage("SetDestination",attackTarget.position);
			}
		} else {
			float baseDistance = Vector3.Distance(enemyBase.transform.position, transform.position);
			if (baseDistance < attackRange) {
				attackTarget = enemyBase.transform;
				ChangeState(State.Attacking);
			} else {
				SendMessage("SetDestination", enemyBase.transform.position);
			}
		}
	}
	
	// 攻撃ステートが始まる前に呼び出される.
	void AttackStart()
	{
		StateStartCommon();
		status.attacking = true;
		
		// 敵の方向に振り向かせる.
		Vector3 targetDirection = (attackTarget.position-transform.position).normalized;
		SendMessage("SetDirection",targetDirection);
		
		// 移動を止める.
		SendMessage("StopMove");
	}
	
	// 攻撃中の処理.
	void Attacking()
	{
		if (charaAnimation.IsAttacked())
			ChangeState(State.Walking);
	}
	
	void Died()
	{
		status.died = true;
		Destroy(gameObject);
	}
	
	void Damage(AttackArea.AttackInfo attackInfo)
	{
		status.HP -= attackInfo.attackPower;
		if (status.HP <= 0) {
			status.HP = 0;
			// 体力０なので死亡ステートへ.
			ChangeState(State.Died);
		}
	}
	
	// ステートが始まる前にステータスを初期化する.
	void StateStartCommon()
	{
		status.attacking = false;
		status.died = false;
	}

    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    GameObject GetNearbyObj(GameObject nowObj, string tagName){
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        GameObject targetObj = null;
        foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(tagName)){
        	tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);
            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis){
                nearDis = tmpDis;
                targetObj = obs;
            }
        }
        //最も近かったオブジェクトを返す
        return targetObj;
    }
}
