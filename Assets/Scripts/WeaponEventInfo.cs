using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponEventInfo : MonoBehaviour
{
    [SerializeField]// �f�o�b�O�p
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

    public bool isChooseWeapon;


    /// <summary>
    /// ����擾�C�x���g�̏����ݒ�
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
    /// ������̏�����
    /// </summary>
    private void InitializeWeaponData() {
        weaponData = null;

        txtWeaponName.text = string.Empty;
        imgWeaponIcon.sprite = null;

        isChooseWeapon = false;
    }

    /// <summary>
    /// ������̃Z�b�g
    /// </summary>
    /// <param name="weaponData"></param>
    public void SetWeaponData(WeaponData weaponData) {
        this.weaponData = weaponData;

        txtWeaponName.text = weaponData.weaponName;
        imgWeaponIcon.sprite = weaponData.weaponIcon;

        SwitchActivateButtons(true);
    }


    private void OnClickSubmit() {

        // ����o�^
        GameData.instance.AddWeaponData(weaponData);
        isChooseWeapon = true;
    }


    private void OnClickCancel() {
        isChooseWeapon = true;
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

        InitializeWeaponData();
    }
}
