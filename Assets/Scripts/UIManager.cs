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
    /// デバッグ内容を画面表示
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
    /// 一時停止処理の実行
    /// </summary>
    private void OnClickStopMotion() {
        autoScroller.StopAndPlayMotion();
    }

    /// <summary>
    /// 一時停止できる残り回数の表示更新
    /// </summary>
    /// <param name="stopMotionCount"></param>
    public void UpdateDisplayStopMotionCount(int stopMotionCount) {
        txtStopMotionCount.text = stopMotionCount.ToString();
    }

    /// <summary>
    /// 分岐のボタン作成
    /// </summary>
    public IEnumerator GenerateBranchButtons(int[] branchNums, BranchDirectionType[] branchDirectionTypes) {

        isSubmitBranch = false;
        Debug.Log("分岐のボタン作成");

        // 分岐の数だけボタンを生成
        for (int i = 0; i < branchNums.Length; i++) {

            // ボタンの生成位置を設定
            Transform branchTran = BranchDirectionType.Right == branchDirectionTypes[i] ? rightBranchTran : BranchDirectionType.Left == branchDirectionTypes[i] ? leftBranchTran : centerBranchTran;
            
            // ボタン生成
            SubmitBranchButton submitBranchButton = Instantiate(submitBranchButtonPrefab, branchTran, false);

            // ボタン設定
            submitBranchButton.SetUpSubmitBranchButton(branchNums[i], this);

            // List に追加
            submitBranchButtonsList.Add(submitBranchButton);
        }
        yield return null;
    }

    /// <summary>
    /// 分岐先の決定
    /// </summary>
    /// <param name="rootNo"></param>
    public void SubmitBranch(int rootNo) {
        for (int i = 0; i < submitBranchButtonsList.Count; i++) {
            // 分岐のボタンを非活性化して重複タップを防止
            submitBranchButtonsList[i].InactivateSubmitButton();
            Destroy(submitBranchButtonsList[i].gameObject);
        }
        submitBranchButtonsList.Clear();

        submitBranchNo = rootNo;
        isSubmitBranch = true;
    }

    /// <summary>
    /// 分岐情報の取得
    /// </summary>
    /// <returns></returns>
    public (bool, int) GetSubmitBranch() {
        return (isSubmitBranch, submitBranchNo);
    }

    /// <summary>
    /// AR 導入部分のメッセージ表示
    /// </summary>
    public void DisplayARIntroduction(string message) {
        txtARIntroduction.text = message;
    }

    /// <summary>
    /// AR 導入用のメッセージ表示のオン/オフ切り替え
    /// </summary>
    /// <param name="isSwicth"></param>
    public void InactiveARIntroductionText(bool isSwicth) {
        txtARIntroduction.transform.parent.parent.gameObject.SetActive(isSwicth);
    }


    /// <summary>
    /// ライフ用のアイコン(パーティクル)を生成
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
    /// ライフの再表示
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
    /// ライフ用アイコンの最大値と弾数の最大値を設定
    /// </summary>
    /// <param name="maxHp"></param>
    public void SetPlayerInfo(int maxHp, int maxBulletCount) {
        maxLifeIcon = maxHp;
        this.maxBulletCount = maxBulletCount;
        UpdateDisplayBulletCount(this.maxBulletCount);
    }

    /// <summary>
    /// 弾数表示の更新
    /// </summary>
    /// <param name="currentBulletCount"></param>
    public void UpdateDisplayBulletCount(int currentBulletCount) {
        txtBulletCount.text = currentBulletCount.ToString() + " / " + maxBulletCount.ToString();
    }
}

