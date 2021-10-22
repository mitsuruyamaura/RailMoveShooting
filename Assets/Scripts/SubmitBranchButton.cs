using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SubmitBranchButton : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit;

    public int rootNo;

    private UIManager uiManager;

    /// <summary>
    /// 分岐ボタンの設定
    /// </summary>
    /// <param name="rootNo"></param>
    /// <param name="uiManager"></param>
    public void SetUpSubmitBranchButton(int rootNo, UIManager uiManager) {
        this.rootNo = rootNo;
        this.uiManager = uiManager;

        btnSubmit.onClick.AddListener(OnClickSubmitBranch);
    }

    /// <summary>
    /// 分岐ボタンをタップした際の処理
    /// </summary>
    private void OnClickSubmitBranch() {
        // TODO アニメ演出

        // 分岐の番号を決定
        uiManager.SubmitBranch(rootNo);
    }

    /// <summary>
    /// 分岐ボタンを非活性化
    /// </summary>
    public void InactivateSubmitButton() {
        btnSubmit.interactable = false;
    }
}
