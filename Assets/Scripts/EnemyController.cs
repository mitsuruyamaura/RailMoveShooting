using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject lookTarget;

    [SerializeField]
    private int enemyNo;

    [SerializeField]
    private int hp;

    [SerializeField]
    private int attackPower;

    [SerializeField]
    private float moveSpeed;

    private Animator anim;

    private bool isAttack;

    private float attackInterval = 3.0f;

    private PlayerController player;

    private GameManager gameManager;

    private IEnumerator attackCoroutine;

    private bool isDead;

    private NavMeshAgent agent;

    private float originMoveSpeed;  // 初期の移動速度の保持用

    public EnemyMoveType enemyMoveType;


    /// <summary>
    /// エネミーの初期設定
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="gameManager"></param>
    public void SetUpEnemy(PlayerController playerController, GameManager gameManager) {

        lookTarget = playerController.gameObject;
        this.gameManager = gameManager;

        TryGetComponent(out anim);

        // NavMesh を利用しているか判定
        if (TryGetComponent(out agent)) {

            // 利用している場合には、初期速度を記録
            originMoveSpeed = moveSpeed;

            // 目標地点をセット
            agent.destination = lookTarget.transform.position;

            // 移動速度を NavMesh に設定
            agent.speed = moveSpeed;

            // アニメがある場合には再生
            if (anim) {
                anim.SetBool("Walk", true);
            }
        }

        Debug.Log("エネミーの設定完了");

        SoundManager.instance.PlaySE(SoundManager.SE_Type.Zombie_1_Enter);
    }

    void Update() {

        // エネミーを対象(カメラ)の方向を向ける
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

    private void OnTriggerStay(Collider other) {
        if (isAttack) {
            return;
        }

        if (other.tag != "MainCamera") {   // 地面のコライダーや、他のエネミーのコライダーも拾ってしまうため
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

                Debug.Log("攻撃範囲内にプレイヤー 初感知");
            }
        }

        // ローカル関数を定義
        void SetAttackCoroutine() {
            // 攻撃用のメソッドを代入して登録
            attackCoroutine = Attack(player);

            // 登録したメソッドを実行
            StartCoroutine(attackCoroutine);

            Debug.Log("攻撃開始");
        }
    }

    private void OnTriggerExit(Collider other) {

        // プレイヤーを感知済みのときに、攻撃範囲内にプレイヤーがいなくなったら
        if (player != null) {

            // 初期化
            player = null;

            // 攻撃処理を止める
            isAttack = false;
            StopCoroutine(attackCoroutine);

            Debug.Log("攻撃範囲外");
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack(PlayerController player = null) {
        isAttack = true;

        SoundManager.instance.PlaySE(SoundManager.SE_Type.Zombie_1_Attack);

        player.CalcHp(-attackPower);

        if (anim) {
            anim.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(attackInterval);

        isAttack = false;
    }

    /// <summary>
    /// 攻撃力取得用
    /// </summary>
    /// <returns></returns>
    public int GetAttackPower() {
        return attackPower;
    }

    /// <summary>
    /// プレイヤーから攻撃を受けた際に呼ばれる処理
    /// </summary>
    /// <param name="damage"></param>
    public void TriggerEvent(int damage) {

        // ダメージ計算
        CalcDamage(damage);
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="damage"></param>
    private void CalcDamage(int damage) {
        if (isDead) {
            return;
        }

        hp -= damage;

        // アニメーションの設定がある場合
        if (anim) anim.ResetTrigger("Attack");

        if (hp <= 0) {
            isDead = true;

            SoundManager.instance.PlaySE(SoundManager.SE_Type.Zombie_1_Down);

            if (anim) {
                anim.SetBool("Walk", false);
                anim.SetBool("Down", true);
            }

            // エネミーの情報を外部クラスの List で管理している場合には、List から削除
            gameManager.RemoveEnemyList(this);

            Destroy(gameObject, 1.5f);
        } else {

            // アニメーションの設定がある場合
            if (anim) anim.SetTrigger("Damage");
        }
    }

    /// <summary>
    /// 移動を一時停止
    /// </summary>
    public void PauseMove() {
        agent.speed = 0;
    }

    /// <summary>
    /// 移動を再開
    /// </summary>
    public void ResumeMove() {
        agent.speed = originMoveSpeed;
    }

    /// <summary>
    /// 目的地を消去
    /// </summary>
    public void ClearPath() {
        agent.ResetPath();
    }
}
