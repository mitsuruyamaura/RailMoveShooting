using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������Ă��镐��̌����p�N���X
/// </summary>
public class WeaponChanger : MonoBehaviour
{
    public int currentWeaponNo;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private WeaponDetail[] weapons;

    /// <summary>
    /// ��������̃��f���\��
    /// </summary>
    public void InitWeaponModel() {

        // ����ɕ���̏����Z�b�g
        SetUpWeaponDetail();

        // ����̕\���X�V
        SwitchWeaponModel();
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {

            // ����̌���
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

        currentWeaponNo = ++currentWeaponNo % GameData.instance.weaponDatasList.Count;

        // ����̃f�[�^���X�V
        player.UpdateCurrentBulletCountData(GameData.instance.weaponDatasList[currentWeaponNo]);

        // ����̕\���X�V
        SwitchWeaponModel();

        Debug.Log("�������");
    }

    /// <summary>
    /// ����̕\���X�V
    /// </summary>
    private void SwitchWeaponModel() {
        // ����̕\��/��\���̐؂�ւ�
        for (int i = 0; i < weapons.Length; i++) {
            if (weapons[i].weaponNo == GameData.instance.weaponDatasList[currentWeaponNo].weaponNo) {
                weapons[i].gameObject.SetActive(true);
            } else {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ����ɕ���̏����Z�b�g(����ł͎g��Ȃ�)
    /// </summary>
    private void SetUpWeaponDetail() {
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].weaponData = GameData.instance.weaponDatasList.Find(x => x.weaponNo == weapons[i].weaponNo);
        }
    }
}
