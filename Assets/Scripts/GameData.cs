using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// �Z�[�u�E���[�h�p�̃N���X
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

    [Header("�������Ă��镐��̓o�^�p���X�g")]
    public List<WeaponData> weaponDatasList = new List<WeaponData>();

    public int chooseStageNo;

    public List<int> clearStageNoList = new List<int>();

    public const string CLEAR_STAGES_KEY = "ClearStageNosList";
    public const string SAVE_KEY = "SaveData";        // SaveData �N���X�p�� Key

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
    /// ����f�[�^�̓o�^
    /// </summary>
    /// <param name="weaponData"></param>
    public void AddWeaponData(WeaponData weaponData) {
        weaponDatasList.Add(weaponData);

        Debug.Log("����ǉ� : " + weaponData.weaponName);
    }

    /// <summary>
    /// �N���A�����X�e�[�W�̔ԍ���ǉ�
    /// </summary>
    /// <param name="stageNo"></param>
    public void AddClearStageNoList(int stageNo) {

        // �N���A�����X�e�[�W�̔ԍ����܂����X�g�ɂȂ��ꍇ
        if (!clearStageNoList.Contains(stageNo)) {
            // List �ɒǉ�
            clearStageNoList.Add(stageNo);

            // �Z�[�u
            SaveClearStageList();
        }
    }

    /// <summary>
    /// �N���A�����X�e�[�W�̔ԍ����Z�[�u
    /// </summary>
    public void SaveClearStageList() {
        string clearStageListStr = string.Empty;

        // List �𕪉��� int �^�� string �^�ɂ���
        for (int i = 0; i < clearStageNoList.Count; i++) {
            clearStageListStr += clearStageNoList[i].ToString() + ",";
        }

        // ������Ƃ��ăZ�[�u
        PlayerPrefsHelper.SaveStringData(CLEAR_STAGES_KEY, clearStageListStr);
    }


    public void LoadClearStageList() {

        // ������Ƃ��ă��[�h
        string clearStageListStr = PlayerPrefsHelper.LoadStringData(CLEAR_STAGES_KEY);

        if (!string.IsNullOrEmpty(clearStageListStr)) {

            string[] strArray = clearStageListStr.Split(new string[] { ","}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < strArray.Length; i++) {
                clearStageNoList.Add(int.Parse(strArray[i]));
            }
        }
    }

    /// <summary>
    /// �Z�[�u����l�� SaveData �ɐݒ肵�ăZ�[�u
    /// �Z�[�u����^�C�~���O�́A�X�e�[�W�N���A��
    /// </summary>
    public void SetSaveData() {

        // �Z�[�u�p�̃f�[�^���쐬
        SaveData saveData = new SaveData {
            // �e�l�� SaveData �N���X�̕ϐ��ɐݒ�
            clearPoint = scoreReactiveProperty.Value,
            weaponDatasList = weaponDatasList,
            clearStageNoList = clearStageNoList
        };

        // SaveData �N���X�Ƃ��� SAVE_KEY �̖��O�ŃZ�[�u
        PlayerPrefsHelper.SaveSetObjectData(SAVE_KEY, saveData);
    }

    /// <summary>
    /// SaveData �����[�h���āA�e�l�ɐݒ�
    /// </summary>
    public void GetSaveData() {

        // SaveData �Ƃ��ă��[�h
        SaveData saveData = PlayerPrefsHelper.LoadGetObjectData<SaveData>(SAVE_KEY);

        // �e�l�� SaveData ���̒l��ݒ�
        scoreReactiveProperty.Value = saveData.clearPoint;
        weaponDatasList = saveData.weaponDatasList;
        clearStageNoList = saveData.clearStageNoList;
    }
}
