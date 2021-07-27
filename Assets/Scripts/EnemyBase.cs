using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

/// <summary>
/// エネミーの移動方法の種類
/// </summary>
public enum EnemyMoveType {
    Agent,
    Boss_0,
    Boss_1
}

/// <summary>
/// エネミーの基幹クラス
/// </summary>
public class EnemyBase : EventBase<int>  //  EventBase<int>    
{
    protected Animator anim;
    protected Tween tween;

    [SerializeField]
    protected GameObject lookTarget;

    [SerializeField]
    protected int enemyNo;

    [SerializeField]
    protected int hp;

    [SerializeField]
    protected int attackPower;

    protected NavMeshAgent agent;

    protected bool isAttack;

    protected float attackInterval = 3.0f;

    protected PlayerController player;

    protected GameManager gameManager;

    protected IEnumerator attackCoroutine;

    protected int point = 100;

    protected bool isDead;

    public EnemyMoveType enemyMoveType;

    // 敵のデータを持たせる


    [SerializeField]
    protected List<BodyRegionPartsController> partsControllersList = new List<BodyRegionPartsController>();


    void Start() {
        // デバッグ用
        SetUpEnemy(lookTarget);
    }

    public virtual void SetUpEnemy(GameObject playerObj, GameManager gameManager = null) {

        lookTarget = playerObj;
        this.gameManager = gameManager;

        // 敵のデータを敵の番号から検索してセット
        GetEnemyData();

        TryGetComponent(out anim);

        // NavMesh を利用しているか判定
        if (TryGetComponent(out agent)) {

            // 利用している場合には目標地点をセット
            agent.destination = lookTarget.transform.position;
                  
            // アニメがある場合には再生
            if (anim) {

                // 移動速度を NavMesh に設定

                anim.SetBool("Walk", true);
            }
        }

        // 部位ごとの情報を設定
        for (int i = 0; i < partsControllersList.Count; i++) {
            //partsControllersList[i].SetUpPartsController(this);
        }
    }

    /// <summary>
    /// 敵の情報をデータベースより取得して設定
    /// </summary>
    protected virtual void GetEnemyData() {

        // データベースからデータを取得してセット
        //enemyData = DataBaseManager.instance.GetEnemyData(enemyNo);

        //hp = enemyData.hp;
        //attackPower = enemyData.attackPower;
        //attackInterval = enemyData.attackInterval;
        //enemyMoveType = enemyData.enemyMoveType;
        //point = enemyData.point;

    }

    protected virtual void Update() {

        // 敵を対象(カメラ)の方向を向ける
        if (lookTarget) {
            Vector3 direction = lookTarget.transform.position - transform.position;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }

        // 目的地を更新
        if (lookTarget != null && agent != null) {
            agent.destination = lookTarget.transform.position;
        }
    }

    protected virtual void OnTriggerStay(Collider other) {
        if (isAttack) {
            return;
        }

        // プレイヤーの情報を保持しており、攻撃中でないなら
        if (player != null) {

            // 攻撃用のメソッドを登録
            SetAttackCoroutine();

            Debug.Log("プレイヤー　感知済");

            // プレイヤーの情報がないなら
        } else {
            if (other.transform.parent.TryGetComponent(out player)) {

                // 攻撃用のメソッドを登録
                SetAttackCoroutine();

                Debug.Log("プレイヤー 初感知");
            }
        }

        // 攻撃用のメソッドを登録
        void SetAttackCoroutine() {
            // 攻撃用のメソッドを代入して登録
            attackCoroutine = Attack(player);

            // 登録したメソッドを実行
            StartCoroutine(attackCoroutine);

            Debug.Log("攻撃開始");
        }
    }

    protected virtual void OnTriggerExit(Collider other) {

        // プレイヤーを感知済みのときに、攻撃範囲内にプレイヤーがいなくなったら
        if (player != null) {

            // 初期化
            player = null;

            // 攻撃処理を止める
            isAttack = false;
            StopCoroutine(attackCoroutine);

            Debug.Log("範囲外");
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Attack(PlayerController player = null) {
        isAttack = true;

        player.CalcHp(-attackPower);

        if (anim) {
            anim.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(attackInterval);

        isAttack = false;
    }

    public int GetAttackPower() {
        return attackPower;
    }

    public override void TriggerEvent(int value) {
        CalcDamage(value);
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="damage"></param>
    public virtual void CalcDamage(int damage, BodyRegionType bodyPartType = BodyRegionType.Boby) {
        if (isDead) {
            return;
        }

    }
}
