using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装備している武器の交換用クラス
/// </summary>
public class WeaponChanger : MonoBehaviour
{
    public int currentWeaponNo;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private WeaponDetail[] weapons;

    /// <summary>
    /// 初期武器のモデル表示
    /// </summary>
    public void InitWeaponModel() {

        // 武器に武器の情報をセット
        SetUpWeaponDetail();

        // 武器の表示更新
        SwitchWeaponModel();
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {

            // 武器の交換
            ChangeWeapon();
        }    
    }

    /// <summary>
    /// 武器の交換
    /// </summary>
    public void ChangeWeapon() {

        // 初期武器しか所持していない場合
        if (GameData.instance.weaponDatasList.Count <= 1) {

            Debug.Log("初期武器しかないため、交換しない");

            // 武器交換しない
            return;
        }

        currentWeaponNo = ++currentWeaponNo % GameData.instance.weaponDatasList.Count;

        // 武器のデータを更新
        player.UpdateCurrentBulletCountData(GameData.instance.weaponDatasList[currentWeaponNo]);

        // 武器の表示更新
        SwitchWeaponModel();

        Debug.Log("武器交換");
    }

    /// <summary>
    /// 武器の表示更新
    /// </summary>
    private void SwitchWeaponModel() {
        // 武器の表示/非表示の切り替え
        for (int i = 0; i < weapons.Length; i++) {
            if (weapons[i].weaponNo == GameData.instance.weaponDatasList[currentWeaponNo].weaponNo) {
                weapons[i].gameObject.SetActive(true);
            } else {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 武器に武器の情報をセット(現状では使わない)
    /// </summary>
    private void SetUpWeaponDetail() {
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].weaponData = GameData.instance.weaponDatasList.Find(x => x.weaponNo == weapons[i].weaponNo);
        }
    }
}
