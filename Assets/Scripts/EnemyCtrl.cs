using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {
    CharacterStatus status;
    CharaAnimation charaAnimation;
    Transform attackTarget;
    // 移動範囲５メートル
    public float walkRange = 5.0f;
    // 初期位置を保存しておく変数
    public Vector3 basePosition;
    public float attackRange = 1.5f;
    GameObject playerBase;
    PlayerCtrl playerCtrl;

    // ステートの種類.
    enum State {
        Walking,	// 探索
        Attacking,	// 攻撃
        Died,       // 死亡
    };

    State state = State.Walking;		// 現在のステート.
    State nextState = State.Walking;	// 次のステート.


    // Use this for initialization
    void Start () {
        status = GetComponent<CharacterStatus>();
        charaAnimation = GetComponent<CharaAnimation>();
        // 初期位置を保持
        basePosition = transform.position;
        playerBase = GameObject.Find("PlayerBase");
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
            Debug.Log(distance);
            if (distance < attackRange) {
                ChangeState(State.Attacking);
            } else {
                SendMessage("SetDestination",attackTarget.position);
            }
        } else {
            SendMessage("SetDestination", playerBase.transform.position);
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
        // ターゲットをリセットする
        attackTarget = null;
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
			// 体力０なので死亡
            ChangeState(State.Died);
        }
    }

	// ステートが始まる前にステータスを初期化する.
	void StateStartCommon()
	{
		status.attacking = false;
        status.died = false;
    }
    // 攻撃対象を設定する
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }
}
