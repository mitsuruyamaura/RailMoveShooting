using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

/// <summary>
/// ボスの行動パターン
/// </summary>
public enum BossActions 
{
    MoveToPosition,
    AttackA,
    AttackB,
    Idle,
    MoveAndAttack
}

/// <summary>
/// ボスの行動パターンごとの重み付け
/// </summary>
[System.Serializable]
public class BossActionWeight
{
    public BossActions Action;
    public int Weight;
}

public class BossBattle : MonoBehaviour
{
    [SerializeField, Header("移動速度")] 
    private float moveSpeed = 10f;

    [SerializeField, Header("次回の行動までの待機時間")] 
    private  float coolDownTime = 5f;
    
    [SerializeField, Header("移動先となる地点")] 
    private Transform[] targetTrans;
    
    [SerializeField, Header("体力")] 
    private  float health;
    
    [SerializeField, Header("移動時のアニメ設定")] 
    private Ease moveEase = Ease.Linear;
    
    [SerializeField, Header("行動パターンごとの重み付け")]
    private List<BossActionWeight> ActionWeights;
    //public List<int> weights = new List<int> {1, 1, 1, 1, 0}; // Initialize the weights for each action(クラスを使わない場合の重み付けに利用する)
    
    [SerializeField, Header("通常の行動パターン")]
    private BossActions[] NormalActionPattern;
    
    [SerializeField, Header("ヘルスが半分以下になった時の行動パターン")]
    private BossActions[] HalfHealthActionPattern;
    
    private bool inCoolDown = false;
    private float initialHealth;
    
    private BossActions currentAction;  // 現在の行動
    public BossActions CurrentAction => currentAction;  // プロパティ
    

    void Start()
    {
        // Initialize the ActionWeights list if it is not initialized from Inspector.
        if(ActionWeights == null || ActionWeights.Count == 0)
        {
            ActionWeights = new List<BossActionWeight>
            {
                new BossActionWeight {Action = BossActions.MoveToPosition, Weight = 1},
                new BossActionWeight {Action = BossActions.AttackA, Weight = 1},
                new BossActionWeight {Action = BossActions.AttackB, Weight = 1},
                new BossActionWeight {Action = BossActions.Idle, Weight = 1},
                new BossActionWeight {Action = BossActions.MoveAndAttack, Weight = 0}
            };
        }
        initialHealth = health;
        
        // ボスの行動開始
        StartCoroutine(BossBehavior());
    }
    
    /// <summary>
    /// ボスの行動
    /// </summary>
    /// <returns></returns>
    private IEnumerator BossBehavior()
    {
        while(true) // Keep this loop running as long as the boss is alive.
        {
            // ヘルスが残っていない場合、行動を終了
            if(health <= 0) 
            {
                break; // Stop the loop if the boss is defeated.
            }

            // ランダムな行動を重み付けしている中から１つ選択
            currentAction = GetRandomActionByWeight();

            // 選択された行動を実行
            switch(currentAction)
            {
                case BossActions.MoveToPosition:
                    // 登録されている位置の中で、ランダムな位置に移動
                    MoveToPosition(targetTrans[Random.Range(0, targetTrans.Length)].position);
                    break;

                case BossActions.AttackA:
                    AttackA();
                    break;

                case BossActions.AttackB:
                    AttackB();
                    break;

                case BossActions.Idle:
                    Idle();
                    break;

                case BossActions.MoveAndAttack:
                    MoveAndAttack(targetTrans[Random.Range(0, targetTrans.Length)].position);
                    break;
            }

            yield return new WaitForSeconds(coolDownTime);
        }
    }
    
    /// <summary>
    /// 重み付けを利用した行動の決定
    /// </summary>
    /// <returns></returns>
    private BossActions GetRandomActionByWeight()
    {
        // 現在のライフの残数に基づいて、適切な行動パターンを選択
        BossActions[] actionPattern = health <= initialHealth / 2 ? HalfHealthActionPattern : NormalActionPattern;

        // その行動パターンに含まれる行動のみの重みを合計
        int totalWeight = ActionWeights.Where(a => actionPattern.Contains(a.Action)).Sum(a => a.Weight);

        // ランダムな行動を選ぶための乱数を取得(対象となる選択肢は選択した行動パターンの行動だけに限定)
        int randomValue = Random.Range(0, totalWeight);
        int weightSum = 0;

        // 乱数から今回の行動を確定(対象となる選択肢は選択した行動パターンの行動だけに限定)
        for (int i = 0; i < ActionWeights.Count; i++) {
            if (!actionPattern.Contains(ActionWeights[i].Action)) {
                continue;
            }

            weightSum += ActionWeights[i].Weight;

            if (randomValue < weightSum) {
                return ActionWeights[i].Action;
            }
        }

        return BossActions.Idle; // Default return value, in case something goes wrong.
    }
    
    private void MoveToPosition(Vector3 target)
    {
        transform.DOMove(target, moveSpeed).SetEase(moveEase).SetSpeedBased();
        Debug.Log("MoveToPosition");
    }

    private void AttackA()
    {
        // TODO Implement AttackA here
        
        Debug.Log("AttackA");
    }

    private void AttackB()
    {
        // TODO Implement AttackB here
        
        Debug.Log("AttackB");
    }

    private void Idle()
    {
        // TODO Idle behavior, do nothing
        
        Debug.Log("Idle");
    }

    private void MoveAndAttack(Vector3 target) {
        
        MoveToPosition(target);
        AttackA();
        
        // TODO Implement Attack here
        
        Debug.Log("MoveAndAttack");
    }
}
