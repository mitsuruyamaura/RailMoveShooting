using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Title : MonoBehaviour
{
    [SerializeField]
    private StageButton stageButtonPrefab;

    [SerializeField]
    private Transform stageButtonTran;

    [SerializeField]
    private List<StageButton> stageButtonList = new List<StageButton>();

    [SerializeField]
    private int[] debugStageNos;

    [SerializeField]
    private bool isLoadData;


    void Start() {
        //TransitionManager.instance.FadeNextScene(1.0f, SceneName.MainGame);

        // デバッグ用。クリアしたステージ番号の登録
        if (debugStageNos.Length > 0) {
            for (int i = 0; i < debugStageNos.Length; i++) {
                GameData.instance.AddClearStageNoList(debugStageNos[i]);
            }
        }

        // クリアしたステージの番号の List をロード
        if (isLoadData) {
            // クリアしたリストの情報がある場合
            if (PlayerPrefsHelper.ExistsData(GameData.CLEAR_STAGES_KEY)) {
                // ロ―ド
                GameData.instance.LoadClearStageList();
            }
        }

        // ステージボタンの作成
        GenerateStageButtons();

        // デバッグ
        this.UpdateAsObservable()
            .Subscribe(_ => {
                if (Input.GetKeyDown(KeyCode.A)) GameData.instance.SaveClearStageList();

                if (Input.GetKeyDown(KeyCode.B)) GameData.instance.SetSaveData();
                if (Input.GetKeyDown(KeyCode.C)) GameData.instance.GetSaveData();
            })
            .AddTo(gameObject);
    }

    /// <summary>
    /// クリアしているステージと次のステージの分のステージ選択ボタンの作成
    /// </summary>
    private void GenerateStageButtons() {

        // １ステージ
        StageButton stageButton = Instantiate(stageButtonPrefab, stageButtonTran, false);
        stageButton.SetUpStageButton(0);
        stageButtonList.Add(stageButton);

        // 2ステージ以降
        for (int i = 0; i < GameData.instance.clearStageNoList.Count; i++) {
            stageButton = Instantiate(stageButtonPrefab, stageButtonTran, false);
            stageButton.SetUpStageButton(GameData.instance.clearStageNoList[i] + 1);
            stageButtonList.Add(stageButton);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            TransitionManager.instance.FadeNextScene(1.0f, SceneName.MainGame);   //  <=  シーン遷移したい列挙子を選択します
        }
    }
}
