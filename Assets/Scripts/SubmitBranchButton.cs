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
    /// ����{�^���̐ݒ�(RootData ���g���Â�����) 
    /// </summary>
    /// <param name="rootNo"></param>
    /// <param name="uiManager"></param>
    public void SetUpSubmitBranchButton(int rootNo, UIManager uiManager) {
        this.rootNo = rootNo;
        this.uiManager = uiManager;

        btnSubmit.onClick.AddListener(OnClickSubmitBranch);
    }

    /// <summary>
    /// ����{�^���̐ݒ�
    /// </summary>
    /// <param name="branchDirectionType"></param>
    /// <param name="uiManager"></param>
    public void SetUpSubmitBranchButton(BranchDirectionType branchDirectionType, UIManager uiManager) {
        this.branchDirectionType = branchDirectionType;
        this.uiManager = uiManager;

        btnSubmit.onClick.AddListener(OnClickSubmitBranch);
    }

    /// <summary>
    /// ����{�^�����^�b�v�����ۂ̏���
    /// </summary>
    private void OnClickSubmitBranch() {
        // �A�j�����o
        transform.DOPunchScale(Vector3.one * 0.3f, 0.25f).SetEase(Ease.InQuart);

        // ����̎�ނ�����
        //uiManager.SubmitBranch(rootNo);
        uiManager.SubmitBranch(branchDirectionType);
    }

    /// <summary>
    /// ����{�^����񊈐���
    /// </summary>
    public void InactivateSubmitButton() {
        btnSubmit.interactable = false;
    }
}
