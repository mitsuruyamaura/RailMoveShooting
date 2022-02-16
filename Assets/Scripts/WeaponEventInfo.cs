using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponEventInfo : MonoBehaviour
{
    [SerializeField]// デバッグ用。あとで SerializeField属性 を削除して private のみにします
    private WeaponData weaponData;

    [SerializeField]
    private Button btnSubmit;　　　// 決定ボタン用

    [SerializeField]
    private Button btnCancel;      // キャンセルボタン用

    [SerializeField]
    private Text txtWeaponName;    // 武器の名称表示用

    [SerializeField]
    private Image imgWeaponIcon;   // 武器のアイコン画像表示用

    [SerializeField]
    private CanvasGroup canvasGroup;

    public bool isChooseWeapon;    // 武器選択の有無


    /// <summary>
    /// 武器取得イベントの初期設定
    /// </summary>
    public void InitializeWeaponEventInfo() {

        SwitchActivateButtons(false);

        btnSubmit.onClick.AddListener(OnClickSubmit);
        btnSubmit.onClick.AddListener(() => SwitchActivateButtons(false));

        btnCancel.onClick.AddListener(OnClickCancel);
        btnCancel.onClick.AddListener(() => SwitchActivateButtons(false));

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

        isChooseWeapon = false;
    }

    /// <summary>
    /// 武器情報のセット
    /// </summary>
    /// <param name="weaponData"></param>
    public void SetWeaponData(WeaponData weaponData) {
        this.weaponData = weaponData;

        txtWeaponName.text = weaponData.weaponName;
        imgWeaponIcon.sprite = weaponData.weaponIcon;

        SwitchActivateButtons(true);
    }

    /// <summary>
    /// 決定ボタンを押下した際の処理
    /// </summary>
    private void OnClickSubmit() {

        // 武器登録
        GameData.instance.AddWeaponData(weaponData);
        isChooseWeapon = true;
    }

    /// <summary>
    /// キャンセルボタンを押下した際の処理
    /// </summary>
    private void OnClickCancel() {
        isChooseWeapon = true;
    }

    /// <summary>
    /// 各ボタンの活性化/非活性化の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateButtons(bool isSwitch) {
        btnSubmit.interactable = isSwitch;
        btnCancel.interactable = isSwitch;
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="duration"></param>
    public void Show(float duration = 0.5f) {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1.0f, duration);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    /// <param name="duration"></param>
    public void Hide(float duration = 0.5f) {
        canvasGroup.DOFade(0, duration).OnComplete(() => gameObject.SetActive(false));

        InitializeWeaponData();
    }
}
