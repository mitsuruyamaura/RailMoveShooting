using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtDebugMessage;


    //mi

    [SerializeField]
    private Button btnStopMotion;

    [SerializeField]
    private FieldAutoScroller autoScroller;

    [SerializeField]
    private Text txtStopMotionCount;

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

    [SerializeField]
    private int submitBranchNo;

    [SerializeField]
    private Text txtARIntroduction;

    [SerializeField]
    private GameObject targetIcon;

    [SerializeField]
    private Transform lifeTran;

    [SerializeField]
    private GameObject lifePrefab;

    [SerializeField]
    private List<GameObject> lifesList = new List<GameObject>();

    [SerializeField]
    private Text txtBulletCount;

    [SerializeField]
    private GameObject playerInfoSet;

    private int maxLifeIcon;
    private int maxBulletCount;

    [SerializeField]
    private Text txtScore;

    /// <summary>
    /// �f�o�b�O���e����ʕ\��
    /// </summary>
    /// <param name="message"></param>
    public void DisplayDebug(string message) {
        txtDebugMessage.text = message;
    }

    public void SwitchActivateTargetIcon(bool isSwitch) {
        targetIcon.SetActive(isSwitch);
    }

    public void SwitchActivatePlayerInfoSet(bool isSwitch) {
        playerInfoSet.SetActive(isSwitch);
    }

    // mi


    void Start() {
        btnStopMotion.onClick.AddListener(OnClickStopMotion);

        GameData.instance.scoreReactiveProperty.Subscribe(x => UpdateDisplayScore(x));
    }

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
    /// ����̃{�^���쐬
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
    public (bool, int) GetSubmitBranch() {
        return (isSubmitBranch, submitBranchNo);
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
    /// ���C�t�p�̃A�C�R��(�p�[�e�B�N��)�𐶐�
    /// </summary>
    public IEnumerator GenerateLife(int amount) {
        
        for (int i = 0; i < amount; i++) {
            lifesList.Add(Instantiate(lifePrefab, lifeTran, false));
            yield return new WaitForSeconds(0.25f);

            if (lifesList.Count == maxLifeIcon) {
                break;
            }
        }
    }

    /// <summary>
    /// ���C�t�̍ĕ\��
    /// </summary>
    /// <param name="amout"></param>
    public void UpdateDisplayLife(int amout) {

        for (int i = 0; i < maxLifeIcon; i++) {

            if (i < amout) {
                lifesList[i].SetActive(true);
            } else {
                lifesList[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// ���C�t�p�A�C�R���̍ő�l�ƒe���̍ő�l��ݒ�
    /// </summary>
    /// <param name="maxHp"></param>
    public void SetPlayerInfo(int maxHp, int maxBulletCount) {
        maxLifeIcon = maxHp;
        this.maxBulletCount = maxBulletCount;
        UpdateDisplayBulletCount(this.maxBulletCount);
    }

    /// <summary>
    /// �e���\���̍X�V
    /// </summary>
    /// <param name="currentBulletCount"></param>
    public void UpdateDisplayBulletCount(int currentBulletCount) {
        txtBulletCount.text = currentBulletCount.ToString() + " / " + maxBulletCount.ToString();
    }
}

