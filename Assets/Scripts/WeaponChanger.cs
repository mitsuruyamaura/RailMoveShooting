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
    /// 武器の交換
    /// </summary>
    public void ChangeWeapon() {

        // 初期武器しか所持していない場合
        if (GameData.instance.weaponDatasList.Count <= 1) {

            Debug.Log("初期武器しかないため、交換しない");

            // 武器交換しない
            return;
        }

        currentWeaponNo = currentWeaponNo++ % GameData.instance.weaponDatasList.Count;

        // 武器のデータを更新
        player.ChangeBulletData(GameData.instance.weaponDatasList[currentWeaponNo]);

        Debug.Log("武器交換");
    }
}
