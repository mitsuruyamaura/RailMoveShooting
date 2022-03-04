using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    [SerializeField]
    private Button btnChoose;

    [SerializeField]
    private Text txtStageName;


    public void SetUpStageButton(int stageNo, string name = null) {

        btnChoose.onClick.AddListener(() => OnClickStageButton(stageNo));
        txtStageName.text = name == null ? "Stage " + (stageNo + 1) : name;
    }


    private void OnClickStageButton(int stageNo) {
        GameData.instance.chooseStageNo = stageNo;

        SceneStateManager.instance.PrepareChangeScene(SceneName.MainGame);
    }
}
