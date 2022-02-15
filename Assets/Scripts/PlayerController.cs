using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �v���C���[���̊Ǘ��N���X
/// </summary>
public class PlayerController : MonoBehaviour
{
    private int hp;                // ���݂�Hp�l

    [SerializeField]
    private int bulletCount;       // ���݂̒e��

    [SerializeField, Header("�ő�Hp�l")]
    private int maxHp;

    [SerializeField, Header("�ő�e���l")]
    private int maxBullet;

    [SerializeField, Header("�����[�h����")]
    private float reloadTime;

    [Header("�e�̍U����")]
    public int bulletPower;

    [Header("�e�̘A�ˑ��x")]
    public float shootInterval;

    [Header("�e�̎˒�����")]
    public float shootRange;

    [Header("�����[�h�@�\�̃I��/�I�t")]
    public bool isReloadModeOn;

    [Header("�����[�h��Ԃ̐���")]
    public bool isReloading;

    [SerializeField]
    private UIManager uiManagaer;

    public int currentWeaponNo;

    [System.Serializable]
    public class BulletCountData {
        public int bulletNo;
        public int bulletCount;

        /// <summary>
        /// �c�e���̍X�V
        /// </summary>
        /// <param name="amount"></param>
        public void SetBulletCount(int amount) {
            bulletCount = amount;
            Debug.Log("�c�e���X�V");
        }
    }

    public List<BulletCountData> bulletCountDatasList = new List<BulletCountData>();


    /// <summary>
    /// �e���p�̃v���p�e�B
    /// </summary>
    public int BulletCount
    {
        set => bulletCount = Mathf.Clamp(value, 0, maxBullet);
        get => bulletCount;
    }

    //HP �������悤�Ƀv���p�e�B���ł���


    // mi

    private bool isShootPermission;

    public bool IsShootPerimission
    {
        set => isShootPermission = value;
        get => isShootPermission;
    }

    [SerializeField]
    private GameObject playerObj;

    [SerializeField]
    private GameObject weaponObj;


    void Start() {
        // Debug �p
        SetUpPlayer();
    }

    /// <summary>
    /// �v���C���[���̏����ݒ�
    /// </summary>
    public void SetUpPlayer() {

        // maxHp �̐ݒ肪���邩�m�F�B�Ȃ���Ώ����l 10 �ŃZ�b�g���� hp ��ݒ�
        hp = maxHp = maxHp == 0 ? 10 : maxHp;

        // maxBullet �̐ݒ肪���邩�m�F�B�Ȃ���Ώ����l 10 �ŃZ�b�g���� �e����ݒ�
        BulletCount = maxBullet = maxBullet == 0 ? 10 : maxBullet;

        // ���̐ݒ�������l�����������������S
        uiManagaer.SetPlayerInfo(maxHp, BulletCount);


        // mi
        IsShootPerimission = true;
         
    }

    /// <summary>
    /// HP�̌v�Z�ƍX�V
    /// </summary>
    public void CalcHp(int amount) {
        hp = Mathf.Clamp(hp += amount, 0, maxHp);

        // HP �\���̍X�V
        uiManagaer.UpdateDisplayLife(hp);

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
    }

    /// <summary>
    /// �e���̌v�Z�ƍX�V
    /// </summary>
    /// <param name="amout"></param>
    public void CalcBulletCount(int amout) {

        BulletCount += amout;

        Debug.Log("���݂̒e�� : " + BulletCount);

        // �e����UI�\���X�V
        //uiManagaer.UpdateDisplayBulletCount(BulletCount);
    }

    /// <summary>
    /// �e���̃����[�h
    /// </summary>
    public IEnumerator ReloadBullet() {

        // �����[�h��Ԃɂ��āA�e�̔��˂𐧌䂷��
        isReloading = true;

        // �����[�h
        BulletCount = maxBullet;

        Debug.Log("�����[�h");

        // �e����UI�\���X�V
        //uiManagaer.UpdateDisplayBulletCount(BulletCount);

        // TODO SE

        // �����[�h�̑ҋ@����
        yield return new WaitForSeconds(reloadTime);

        // �ēx�A�e�����˂ł����Ԃɂ���
        isReloading = false;
    }

    private void OnTriggerEnter(Collider other) {

        // �G����̍U���ɂ���Ĕ�e�����ꍇ�̏���

        // �{�X��G�̍U���͈͂����m���Ȃ��悤�ɂ��邽�߂Ƀ^�O�ł����肷�邩�A���C���[��ݒ肵�ĉ������B�����ł̓^�O�ŊǗ�
        if (other.gameObject.tag == "Bullet" && other.transform.parent.gameObject.TryGetComponent(out Bullet bullet)) {
            CalcHp(-bullet.attackPower);

            Destroy(other.gameObject);

            Debug.Log("�G�̍U�����v���C���[�Ƀq�b�g");
        }
    }

    /// <summary>
    /// ����(�o���b�g)�̏���ύX���ĕ���؂�ւ�
    /// </summary>
    /// <param name="weaponData"></param>
    public void ChangeBulletData(WeaponData weaponData) {

        // ���ݎg�p���Ă��镐��̔ԍ���ێ�
        currentWeaponNo = weaponData.weaponNo;

        // ����̏����e�ϐ��ɐݒ�
        bulletPower = weaponData.bulletPower;
        maxBullet = weaponData.maxBullet;

        reloadTime = weaponData.reloadTime;
        shootInterval = weaponData.shootInterval;
        shootRange = weaponData.shootRange;

        bulletCount = maxBullet;

        // ���łɎg�p�������Ƃ̂��镐��ł���ꍇ
        if (bulletCountDatasList.Exists(x =>  x.bulletNo == currentWeaponNo)) {

            // �e����O��̎c�e���ɂ���
            bulletCount = bulletCountDatasList.Find(x => x.bulletNo == currentWeaponNo).bulletCount;
        }
    }

    /// <summary>
    /// �g�p���Ă��镐��̃f�[�^(����̔ԍ��Ǝc�e��)���L�^���čX�V
    /// </summary>
    /// <param name="weaponData"></param>
    public void UpdateCurrentBulletCountData(WeaponData weaponData) {

        // �܂���x���g�p���Ă��镐��̎c�e�����L�^���Ă��Ȃ��Ƃ�
        if (!bulletCountDatasList.Exists(x => x.bulletNo == currentWeaponNo)) {
            // �V�����f�[�^���쐬���āA�L�^
            bulletCountDatasList.Add(new BulletCountData { bulletNo = currentWeaponNo, bulletCount = bulletCount });
        } else {
            // �g�p�������Ƃ����镐��ł��łɃf�[�^������ꍇ�ɂ́A���̃f�[�^�������ď㏑�����ċL�^
            bulletCountDatasList.Find(x => x.bulletNo == currentWeaponNo).SetBulletCount(bulletCount);
        }

        // ����̏���ݒ�
        ChangeBulletData(weaponData);
    }

    /// <summary>
    /// �Q�[���N���A���̉��o�̏���
    /// </summary>
    public void PrepareClearSettings() {
        // �v���C���[�̃Q�[���I�u�W�F�N�g�Ƃ̐e�q�֌W�̉���
        playerObj.transform.SetParent(null);
        // ������\��
        weaponObj.SetActive(false);
    }
}
