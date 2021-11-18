using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    public int currentWeaponNo;

    [SerializeField]
    private PlayerController player;


    void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            ChangeWeapon();
        }    
    }

    /// <summary>
    /// ����̌���
    /// </summary>
    public void ChangeWeapon() {

        // �������킵���������Ă��Ȃ��ꍇ
        if (GameData.instance.weaponDatasList.Count <= 1) {

            Debug.Log("�������킵���Ȃ����߁A�������Ȃ�");

            // ����������Ȃ�
            return;
        }

        currentWeaponNo = currentWeaponNo++ % GameData.instance.weaponDatasList.Count;

        // ����̃f�[�^���X�V
        player.ChangeBulletData(GameData.instance.weaponDatasList[currentWeaponNo]);

        Debug.Log("�������");
    }
}
