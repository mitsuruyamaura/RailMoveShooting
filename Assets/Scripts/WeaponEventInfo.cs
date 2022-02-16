using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponEventInfo : MonoBehaviour
{
    [SerializeField]// �f�o�b�O�p�B���Ƃ� SerializeField���� ���폜���� private �݂̂ɂ��܂�
    private WeaponData weaponData;

    [SerializeField]
    private Button btnSubmit;�@�@�@// ����{�^���p

    [SerializeField]
    private Button btnCancel;      // �L�����Z���{�^���p

    [SerializeField]
    private Text txtWeaponName;    // ����̖��̕\���p

    [SerializeField]
    private Image imgWeaponIcon;   // ����̃A�C�R���摜�\���p

    [SerializeField]
    private CanvasGroup canvasGroup;

    public bool isChooseWeapon;    // ����I���̗L��


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

    /// <summary>
    /// ����{�^�������������ۂ̏���
    /// </summary>
    private void OnClickSubmit() {

        // ����o�^
        GameData.instance.AddWeaponData(weaponData);
        isChooseWeapon = true;
    }

    /// <summary>
    /// �L�����Z���{�^�������������ۂ̏���
    /// </summary>
    private void OnClickCancel() {
        isChooseWeapon = true;
    }

    /// <summary>
    /// �e�{�^���̊�����/�񊈐����̐؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateButtons(bool isSwitch) {
        btnSubmit.interactable = isSwitch;
        btnCancel.interactable = isSwitch;
    }

    /// <summary>
    /// �\��
    /// </summary>
    /// <param name="duration"></param>
    public void Show(float duration = 0.5f) {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1.0f, duration);
    }

    /// <summary>
    /// ��\��
    /// </summary>
    /// <param name="duration"></param>
    public void Hide(float duration = 0.5f) {
        canvasGroup.DOFade(0, duration).OnComplete(() => gameObject.SetActive(false));

        InitializeWeaponData();
    }
}
