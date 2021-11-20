using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponEventInfo : MonoBehaviour
{
    [SerializeField]// デバッグ用
    private WeaponData weaponData;

    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    private Button btnCancel;

    [SerializeField]
    private Text txtWeaponName;

    [SerializeField]
    private Image imgWeaponIcon;

    [SerializeField]
    private CanvasGroup canvasGroup;


    /// <summary>
    /// 武器取得イベントの初期設定
    /// </summary>
    public void InitializeWeaponEventInfo() {

        SwitchActivateButtons(false);

        btnSubmit.onClick.AddListener(OnClickSubmit);
        btnSubmit.onClick.AddListener(() => SwitchActivateButtons(false));

        btnCancel.onClick.AddListener(OnClickCancel);
        btnCancel.onClick.AddListener(() => SwitchActivateButtons(false));
        Debug.Log("ボタンにメソッド登録");

        SwitchActivateButtons(true);

        Hide(0);
    }

    /// <summary>
    /// 武器情報の初期化
    /// </summary>
    private void InitializeWeaponData() {
        weaponData = null;

        txtWeaponName.text = string.Empty;
        imgWeaponIcon.sprite = null;
    }

    /// <summary>
    /// 武器情報のセット
    /// </summary>
    /// <param name="weaponData"></param>
    public void SetWeaponData(WeaponData weaponData) {
        this.weaponData = weaponData;

        txtWeaponName.text = weaponData.weaponName;
        imgWeaponIcon.sprite = weaponData.weaponIcon;
    }


    private void OnClickSubmit() {

        // 武器登録
        GameData.instance.AddWeaponData(weaponData);

        InitializeWeaponData();
        Hide();
    }


    private void OnClickCancel() {

        InitializeWeaponData();
        Hide();
    }


    private void SwitchActivateButtons(bool isSwitch) {
        btnSubmit.interactable = isSwitch;
        btnCancel.interactable = isSwitch;
    }


    public void Show(float duration = 0.5f) {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1.0f, duration);
    }


    public void Hide(float duration = 0.5f) {
        canvasGroup.DOFade(0, duration).OnComplete(() => gameObject.SetActive(false));
    }
}
