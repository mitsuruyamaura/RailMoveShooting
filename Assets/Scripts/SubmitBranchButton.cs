using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SubmitBranchButton : MonoBehaviour
{
    [SerializeField]
    private Button btnSubmit;

    private int rootNo;

    private BranchDirectionType branchDirectionType;

    private UIManager uiManager;

    /// <summary>
    /// 分岐ボタンの設定(RootData を使う古い方式) 
    /// </summary>
    /// <param name="rootNo"></param>
    /// <param name="uiManager"></param>
    public void SetUpSubmitBranchButton(int rootNo, UIManager uiManager) {
        this.rootNo = rootNo;
        this.uiManager = uiManager;

        btnSubmit.onClick.AddListener(OnClickSubmitBranch);
    }

    /// <summary>
    /// 分岐ボタンの設定
    /// </summary>
    /// <param name="branchDirectionType"></param>
    /// <param name="uiManager"></param>
    public void SetUpSubmitBranchButton(BranchDirectionType branchDirectionType, UIManager uiManager) {
        this.branchDirectionType = branchDirectionType;
        this.uiManager = uiManager;

        btnSubmit.onClick.AddListener(OnClickSubmitBranch);
    }

    /// <summary>
    /// 分岐ボタンをタップした際の処理
    /// </summary>
    private void OnClickSubmitBranch() {
        // アニメ演出
        transform.DOPunchScale(Vector3.one * 0.3f, 0.25f).SetEase(Ease.InQuart);

        // 分岐の種類を決定
        //uiManager.SubmitBranch(rootNo);
        uiManager.SubmitBranch(branchDirectionType);
    }

    /// <summary>
    /// 分岐ボタンを非活性化
    /// </summary>
    public void InactivateSubmitButton() {
        btnSubmit.interactable = false;
    }
}
