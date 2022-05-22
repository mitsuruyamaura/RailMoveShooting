using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Transform lifeTran;

    [SerializeField]
    private GameObject lifePrefab;

    [SerializeField]
    private List<GameObject> lifesList = new List<GameObject>();

    [SerializeField]
    private GameObject playerInfoSet;

    //private int maxLifeIcon;

    [SerializeField]
    private SubmitBranchButton submitBranchButtonPrefab;

    [SerializeField]
    private List<SubmitBranchButton> submitBranchButtonsList = new List<SubmitBranchButton>();

    [SerializeField]
    private Transform rightBranchTran;

    [SerializeField]
    private Transform leftBranchTran;

    [SerializeField]
    private Transform centerBranchTran;

    [SerializeField]
    private bool isSubmitBranch;

    private int submitBranchNo;�@�@�@//�@RootData �p�̌Â��ϐ�

    private BranchDirectionType chooseBranchDirectionType;

    [SerializeField]
    private GameObject canvasObj;

    [SerializeField]
    private Button btnWeaponChange;

    [SerializeField]
    private GameObject targetIcon;


    //mi

    [SerializeField]
    private Text txtDebugMessage;

    [SerializeField]
    private Button btnStopMotion;

    [SerializeField]
    private FieldAutoScroller autoScroller;

    [SerializeField]
    private Text txtStopMotionCount;

    [SerializeField]
    private Text txtARIntroduction;

    [SerializeField]
    private Text txtBulletCount;

    private int maxBulletCount;

    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private ResultCanvas resultCanvas;


    /// <summary>
    /// ���C�t�p�A�C�R���̍ő�l�ƒe���̍ő�l��ݒ�
    /// </summary>
    /// <param name="maxHp"></param>
    public void SetPlayerInfo(int maxHp, int maxBulletCount) {  // TODO ��2�����͂܂�

        StartCoroutine(GenerateLife(maxHp));

        //this.maxBulletCount = maxBulletCount;
        //UpdateDisplayBulletCount(this.maxBulletCount);

        SwitchActivateTargetIcon(GameData.instance.isTargetMarker);
    }

    /// <summary>
    /// ���C�t�p�̃A�C�R��(�p�[�e�B�N��)�𐶐�
    /// </summary>
    /// <param name="lifeCount"></param>
    /// <returns></returns>
    public IEnumerator GenerateLife(int lifeCount) {

        for (int i = 0; i < lifeCount; i++) {
            yield return new WaitForSeconds(0.25f);

            lifesList.Add(Instantiate(lifePrefab, lifeTran, false));

        }
    }

    /// <summary>
    /// ���C�t�̍ĕ\��
    /// </summary>
    /// <param name="amout"></param>
    public void UpdateDisplayLife(int amout) {

        for (int i = 0; i < lifesList.Count; i++) {

            if (i < amout) {
                lifesList[i].SetActive(true);
            } else {
                lifesList[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// ����{�^���̐���
    /// </summary>
    /// <param name="branchDirectionTypes"></param>
    public void GenerateBranchButtons(BranchDirectionType[] branchDirectionTypes) {
        isSubmitBranch = false;
        Debug.Log("����̃{�^���쐬");

        // ����̐������{�^���𐶐�
        for (int i = 0; i < branchDirectionTypes.Length; i++) {

            // �{�^���̐����ʒu��ݒ�
            Transform branchTran = BranchDirectionType.Right == branchDirectionTypes[i] ? rightBranchTran : BranchDirectionType.Left == branchDirectionTypes[i] ? leftBranchTran : centerBranchTran;

            // �{�^������
            SubmitBranchButton submitBranchButton = Instantiate(submitBranchButtonPrefab, branchTran, false);

            // �{�^���ݒ�
            submitBranchButton.SetUpSubmitBranchButton(branchDirectionTypes[i], this);

            // List �ɒǉ�
            submitBranchButtonsList.Add(submitBranchButton);
        }
    }

    /// <summary>
    /// �����̌���
    /// </summary>
    /// <param name="branchDirectionType"></param>
    public void SubmitBranch(BranchDirectionType branchDirectionType) {
        for (int i = 0; i < submitBranchButtonsList.Count; i++) {
            // ����̃{�^����񊈐������ďd���^�b�v��h�~
            submitBranchButtonsList[i].InactivateSubmitButton();
            Destroy(submitBranchButtonsList[i].gameObject);
        }
        submitBranchButtonsList.Clear();

        chooseBranchDirectionType = branchDirectionType;
        isSubmitBranch = true;
    }

    /// <summary>
    /// ������̎擾
    /// </summary>
    /// <returns></returns>
    public (bool, BranchDirectionType) GetSubmitBranch() {
        return (isSubmitBranch, chooseBranchDirectionType);
    }

    /// <summary>
    /// �L�����o�X�̕\���I���I�t�؂�ւ�
    /// </summary>
    public void SwitchActivateCanvas(bool isSwitch) {
        canvasObj.SetActive(isSwitch);
    }

    /// <summary>
    /// ��������{�^���̎擾
    /// </summary>
    /// <returns></returns>
    public Button GetWeaponChangeButton() {
        return btnWeaponChange;
    }


    // mi

    /// <summary>
    /// �f�o�b�O���e����ʕ\��
    /// </summary>
    /// <param name="message"></param>
    public void DisplayDebug(string message) {
        txtDebugMessage.text = message;
    }

    /// <summary>
    /// �^�[�Q�b�g�}�[�J�[�̃I���I�t�؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateTargetIcon(bool isSwitch) {
        targetIcon.SetActive(isSwitch);
    }

    void Update() {

        if (targetIcon != null && targetIcon.activeSelf) {
            // �}�E�X�̈ʒu�Ƀ^�[�Q�b�g�}�[�J�[���ړ�
            targetIcon.transform.position = Input.mousePosition;
        }
    }


    public void SwitchActivatePlayerInfoSet(bool isSwitch) {
        playerInfoSet.SetActive(isSwitch);
    }


    // �{���͎g���B���܂̓f�o�b�O�̂��ߖ��g�p

    //void Start() {
    //    btnStopMotion.onClick.AddListener(OnClickStopMotion);

    //    GameData.instance.scoreReactiveProperty.Subscribe(x => UpdateDisplayScore(x));
    //}

    private void UpdateDisplayScore(int scoreValue) {
        txtScore.text = scoreValue.ToString();
    }

    /// <summary>
    /// �ꎞ��~�����̎��s
    /// </summary>
    private void OnClickStopMotion() {
        autoScroller.StopAndPlayMotion();
    }

    /// <summary>
    /// �ꎞ��~�ł���c��񐔂̕\���X�V
    /// </summary>
    /// <param name="stopMotionCount"></param>
    public void UpdateDisplayStopMotionCount(int stopMotionCount) {
        txtStopMotionCount.text = stopMotionCount.ToString();
    }

    /// <summary>
    /// AR ���������̃��b�Z�[�W�\��
    /// </summary>
    public void DisplayARIntroduction(string message) {
        txtARIntroduction.text = message;
    }

    /// <summary>
    /// AR �����p�̃��b�Z�[�W�\���̃I��/�I�t�؂�ւ�
    /// </summary>
    /// <param name="isSwicth"></param>
    public void InactiveARIntroductionText(bool isSwicth) {
        txtARIntroduction.transform.parent.parent.gameObject.SetActive(isSwicth);
    }

    /// <summary>
    /// �e���\���̍X�V
    /// </summary>
    /// <param name="currentBulletCount"></param>
    public void UpdateDisplayBulletCount(int currentBulletCount) {
        txtBulletCount.text = currentBulletCount.ToString() + " / " + maxBulletCount.ToString();
    }


    // ��

    /// <summary>
    /// ����̃{�^���쐬(RootData ���g���A�Â�����)
    /// </summary>
    public IEnumerator GenerateBranchButtons(int[] branchNums, BranchDirectionType[] branchDirectionTypes) {

        isSubmitBranch = false;
        Debug.Log("����̃{�^���쐬");

        // ����̐������{�^���𐶐�
        for (int i = 0; i < branchNums.Length; i++) {

            // �{�^���̐����ʒu��ݒ�
            Transform branchTran = BranchDirectionType.Right == branchDirectionTypes[i] ? rightBranchTran : BranchDirectionType.Left == branchDirectionTypes[i] ? leftBranchTran : centerBranchTran;

            // �{�^������
            SubmitBranchButton submitBranchButton = Instantiate(submitBranchButtonPrefab, branchTran, false);

            // �{�^���ݒ�
            submitBranchButton.SetUpSubmitBranchButton(branchNums[i], this);

            // List �ɒǉ�
            submitBranchButtonsList.Add(submitBranchButton);
        }
        yield return null;
    }

    /// <summary>
    /// �����̌���
    /// </summary>
    /// <param name="rootNo"></param>
    public void SubmitBranch(int rootNo) {
        for (int i = 0; i < submitBranchButtonsList.Count; i++) {
            // ����̃{�^����񊈐������ďd���^�b�v��h�~
            submitBranchButtonsList[i].InactivateSubmitButton();
            Destroy(submitBranchButtonsList[i].gameObject);
        }
        submitBranchButtonsList.Clear();

        submitBranchNo = rootNo;
        isSubmitBranch = true;
    }

    /// <summary>
    /// ������̎擾
    /// </summary>
    /// <returns></returns>
    public (bool, int) GetSubmitBranchNo() {
        return (isSubmitBranch, submitBranchNo);
    }


    public void GenerateResultCanvas(int score, int secretPoint) {
        Instantiate(resultCanvas).SetUpResultCanvas(score, secretPoint);
    }
}