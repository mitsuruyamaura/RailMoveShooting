using UnityEngine;

/// <summary>
/// エネミーの部位ごとのダメージを計算するクラス
/// </summary>
public class BodyRegionPartsController : MonoBehaviour
{
    [SerializeField, Header("部位の設定")]
    private BodyRegionType bodyPartType;

    //private EnemyBase enemyController;


    //public void SetUpPartsController(EnemyBase enemyController) {
    //    this.enemyController = enemyController;
    //}

    /// <summary>
    /// 部位の情報の取得用。プロパティでも可
    /// </summary>
    /// <returns></returns>
    public BodyRegionType GetBodyPartType() {
        return bodyPartType;
    }

    /// <summary>
    /// 部位ごとにダメージの値を計算
    /// </summary>
    /// <param name="damage"></param>
    public (int, BodyRegionType) CalcDamageParts(int damage) {

        int lastDamage = bodyPartType switch {
            BodyRegionType.Head => damage * 5,

            // TODO 他の部位を追加

            _ => damage * 1,
        };

        return (lastDamage, bodyPartType); 

        //enemyController.CalcDamage(lastDamage, bodyPartType);
    }
}
