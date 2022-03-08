using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [HideInInspector]
    public ReactiveProperty<int> scoreReactiveProperty;

    [HideInInspector]
    public bool useCinemachine;

    [Header("所持している武器の登録用リスト")]
    public List<WeaponData> weaponDatasList = new List<WeaponData>();

    public int chooseStageNo;

    public List<int> clearStageNoList = new List<int>();


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void InitializeGameData() {
        scoreReactiveProperty = new ReactiveProperty<int>();
    }

    /// <summary>
    /// 武器データの登録
    /// </summary>
    /// <param name="weaponData"></param>
    public void AddWeaponData(WeaponData weaponData) {
        weaponDatasList.Add(weaponData);

        Debug.Log("武器追加 : " + weaponData.weaponName);
    }

    /// <summary>
    /// クリアしたステージの番号を追加
    /// </summary>
    /// <param name="stageNo"></param>
    public void AddClearStageNoList(int stageNo) {

        // クリアしたステージの番号がまだリストにない場合
        if (!clearStageNoList.Contains(stageNo)) {

            // List に追加
            clearStageNoList.Add(stageNo);

            // セーブ
            PlayerPrefsHelper.SaveSetObjectData("ClearStageNoList", clearStageNoList);
        }
    }
}
