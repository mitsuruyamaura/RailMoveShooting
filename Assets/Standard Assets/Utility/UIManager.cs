using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text txtDebugMessage;

    [SerializeField]
    private Transform lifeTran;

    [SerializeField]
    private GameObject lifePrefab;

    [SerializeField]
    private List<GameObject> lifesList = new List<GameObject>();

    private int maxLifeIcon;


    /// <summary>
    /// デバッグ内容を画面表示
    /// </summary>
    /// <param name="message"></param>
    public void DisplayDebug(string message) {
        txtDebugMessage.text = message;
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
        
        //this.maxBulletCount = maxBulletCount;
        //UpdateDisplayBulletCount(this.maxBulletCount);
    }
}


