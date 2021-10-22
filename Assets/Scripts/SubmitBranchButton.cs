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
    /// ����{�^���̐ݒ�
    /// </summary>
    /// <param name="rootNo"></param>
    /// <param name="uiManager"></param>
    public void SetUpSubmitBranchButton(int rootNo, UIManager uiManager) {
        this.rootNo = rootNo;
        this.uiManager = uiManager;

        btnSubmit.onClick.AddListener(OnClickSubmitBranch);
    }

    /// <summary>
    /// ����{�^�����^�b�v�����ۂ̏���
    /// </summary>
    private void OnClickSubmitBranch() {
        // TODO �A�j�����o

        // ����̔ԍ�������
        uiManager.SubmitBranch(rootNo);
    }

    /// <summary>
    /// ����{�^����񊈐���
    /// </summary>
    public void InactivateSubmitButton() {
        btnSubmit.interactable = false;
    }
}
