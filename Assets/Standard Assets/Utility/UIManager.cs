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
    /// �f�o�b�O���e����ʕ\��
    /// </summary>
    /// <param name="message"></param>
    public void DisplayDebug(string message) {
        txtDebugMessage.text = message;
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
        
        //this.maxBulletCount = maxBulletCount;
        //UpdateDisplayBulletCount(this.maxBulletCount);
    }
}


