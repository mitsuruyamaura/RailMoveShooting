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

    [Header("�������Ă��镐��̓o�^�p���X�g")]
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
            PlayerPrefsHelper.SaveSetObjectData("ClearStageNoList", clearStageNoList);
        }
    }
}
