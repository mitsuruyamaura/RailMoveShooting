using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    LifeUp,
    Bullet,
    ScoreUp,
}


public class ItemController : EventBase
{
    public ItemType itemType;   // TODO 配列にして複数の効果の設定を可能にする

    public int itemAmout;

    private PlayerController playerController;

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="gameManager"></param>
    public override void SetUpEvent(PlayerController playerController, GameManager gameManager) {

        this.playerController = playerController;
    }


    ///// <summary>
    ///// 設定
    ///// </summary>
    ///// <param name="playerController"></param>
    //public void SetUpItem(PlayerController playerController) {
    //    this.playerController = playerController;
    //}

    public override void TriggerEvent(int value, BodyRegionType hitBodyRegionType) {
        ItemEffect(value);
    }

    /// <summary>
    /// アイテムの効果
    /// </summary>
    /// <param name="value"></param>
    private void ItemEffect(int value) {
        switch (itemType) {
            case ItemType.LifeUp:
                playerController.CalcHp(itemAmout);
                break;

            case ItemType.Bullet:
                playerController.CalcBulletCount(itemAmout);
                break;

            case ItemType.ScoreUp:
                GameData.instance.scoreReactiveProperty.Value += itemAmout;
                break;
        }

        Destroy(gameObject);
    }
}
