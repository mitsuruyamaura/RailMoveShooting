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
    private int bulletCount;       // ���݂̒e���n

    [SerializeField, HideInInspector]
    private UIManager uiManagaer;

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

    public int currentBulletNo;

    [System.Serializable]
    public struct CurrentBulletCount {
        public int bulletNo;
        public int bulletCount;
    }

    public List<CurrentBulletCount> currentBulletCountsList = new List<CurrentBulletCount>();


    /// <summary>
    /// �e���p�̃v���p�e�B
    /// </summary>
    public int BulletCount
    {
        set => bulletCount = Mathf.Clamp(value, 0, maxBullet);
        get => bulletCount;        
    }

    //HP �������悤�Ƀv���p�e�B���ł���


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

    }

    /// <summary>
    /// HP�̌v�Z�ƍX�V
    /// </summary>
    public void CalcHp(int amount) {
        hp = Mathf.Clamp(hp += amount, 0, maxHp);

        // HP �\���̍X�V
        //uiManagaer.UpdateDisplayLife(hp);

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
    /// �o���b�g�̏����X�V
    /// </summary>
    /// <param name="weaponData"></param>
    public void ChangeBulletData(WeaponData weaponData) {

        // ��������ȊO�̏ꍇ
        if (weaponData.weaponNo != 0) {

            // �e���c���Ă���ꍇ�ŁA�܂��L�^����Ă��Ȃ��Ƃ�
            if (bulletCount > 0 && !currentBulletCountsList.Exists(x => x.bulletNo == currentBulletNo)) {

                // �L�^���Ă���
                currentBulletCountsList.Add(new CurrentBulletCount { bulletNo = currentBulletNo, bulletCount = bulletCount});
            }
        }

        bulletPower = weaponData.bulletPower;
        bulletCount = weaponData.maxBullet;

        reloadTime = weaponData.reloadTime;
        shootInterval = weaponData.shootInterval;
        shootRange = weaponData.shootRange;

        // �L�^����Ă���e��������ꍇ�ɂ́A������ɂ���
        bulletCount = currentBulletCountsList.Find(x => x.bulletNo == weaponData.weaponNo).bulletCount;

    }
}
