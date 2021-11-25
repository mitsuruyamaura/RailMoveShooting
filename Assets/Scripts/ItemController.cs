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
    public ItemType itemType;   // TODO �z��ɂ��ĕ����̌��ʂ̐ݒ���\�ɂ���

    public int itemAmout;

    private PlayerController playerController;

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="gameManager"></param>
    public override void SetUpEvent(PlayerController playerController, GameManager gameManager) {

        this.playerController = playerController;
    }


    ///// <summary>
    ///// �ݒ�
    ///// </summary>
    ///// <param name="playerController"></param>
    //public void SetUpItem(PlayerController playerController) {
    //    this.playerController = playerController;
    //}

    public override void TriggerEvent(int value, BodyRegionType hitBodyRegionType) {
        ItemEffect(value);
    }

    /// <summary>
    /// �A�C�e���̌���
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
