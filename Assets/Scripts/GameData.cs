using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// セーブ・ロード用のクラス
/// </summary>
[System.Serializable]
public class SaveData {
    public int clearPoint;
    public List<WeaponData> weaponDatasList = new List<WeaponData>();
    public List<int> clearStageNoList = new List<int>();
}


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

    public const string CLEAR_STAGES_KEY = "ClearStageNosList";
    public const string SAVE_KEY = "SaveData";        // SaveData クラス用の Key

    public bool isTargetMarker;


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
            SaveClearStageList();
        }
    }

    /// <summary>
    /// クリアしたステージの番号をセーブ
    /// </summary>
    public void SaveClearStageList() {
        string clearStageListStr = string.Empty;

        // List を分解し int 型を string 型にする
        for (int i = 0; i < clearStageNoList.Count; i++) {
            clearStageListStr += clearStageNoList[i].ToString() + ",";
        }

        // 文字列としてセーブ
        PlayerPrefsHelper.SaveStringData(CLEAR_STAGES_KEY, clearStageListStr);
    }


    public void LoadClearStageList() {

        // 文字列としてロード
        string clearStageListStr = PlayerPrefsHelper.LoadStringData(CLEAR_STAGES_KEY);

        if (!string.IsNullOrEmpty(clearStageListStr)) {

            string[] strArray = clearStageListStr.Split(new string[] { ","}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < strArray.Length; i++) {
                clearStageNoList.Add(int.Parse(strArray[i]));
            }
        }
    }

    /// <summary>
    /// セーブする値を SaveData に設定してセーブ
    /// セーブするタイミングは、ステージクリア時
    /// </summary>
    public void SetSaveData() {

        // セーブ用のデータを作成
        SaveData saveData = new SaveData {
            // 各値を SaveData クラスの変数に設定
            clearPoint = scoreReactiveProperty.Value,
            weaponDatasList = weaponDatasList,
            clearStageNoList = clearStageNoList
        };

        // SaveData クラスとして SAVE_KEY の名前でセーブ
        PlayerPrefsHelper.SaveSetObjectData(SAVE_KEY, saveData);
    }

    /// <summary>
    /// SaveData をロードして、各値に設定
    /// </summary>
    public void GetSaveData() {

        // SaveData としてロード
        SaveData saveData = PlayerPrefsHelper.LoadGetObjectData<SaveData>(SAVE_KEY);

        // 各値に SaveData 内の値を設定
        scoreReactiveProperty.Value = saveData.clearPoint;
        weaponDatasList = saveData.weaponDatasList;
        clearStageNoList = saveData.clearStageNoList;
    }
}
