using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�l�~�[�̊�N���X
/// </summary>
public class EnemyBase : EventBase    
{
    [SerializeField]
    protected GameObject lookTarget;

    [SerializeField]
    protected int enemyNo;

    [SerializeField]
    protected int hp;

    [SerializeField]
    protected int attackPower;

    [SerializeField]
    protected float moveSpeed;

    protected Animator anim;

    protected bool isAttack;

    protected float attackInterval = 3.0f;

    protected PlayerController player;

    protected GameManager gameManager;

    protected IEnumerator attackCoroutine;

    protected int point = 100;

    protected bool isDead;

    public EnemyMoveType enemyMoveType;

    [SerializeField, Header("���ʂ̏���o�^���郊�X�g")]
    protected List<BodyRegionPartsController> partsControllersList = new List<BodyRegionPartsController>();

    // TODO �G�̃f�[�^�̃N���X����������



    protected virtual void Start() {
        // �f�o�b�O�p
        SetUpEnemy(lookTarget);
    }

    /// <summary>
    /// �G�l�~�[�̐ݒ�B�O���N���X����Ăяo���݌v
    /// </summary>
    /// <param name="playerObj"></param>
    /// <param name="gameManager"></param>
    public virtual void SetUpEnemy(GameObject playerObj, GameManager gameManager = null) {

        lookTarget = playerObj;
        this.gameManager = gameManager;

        // �G�̃f�[�^��G�̔ԍ����猟�����ăZ�b�g
        GetEnemyData();

        TryGetComponent(out anim);

        //// ���ʂ��Ƃ̏�񂪂��邩�m�F
        //if (partsControllersList.Count > 0) {
        //    // ���ʂ̏���ݒ�
        //    SetBodyParts();
        //}
    }

    /// <summary>
    /// �G�̏����f�[�^�x�[�X���擾���Đݒ�
    /// </summary>
    protected virtual void GetEnemyData() {

        // �f�[�^�x�[�X����f�[�^���擾���ăZ�b�g
        //enemyData = DataBaseManager.instance.GetEnemyData(enemyNo);

        //hp = enemyData.hp;
        //attackPower = enemyData.attackPower;
        //attackInterval = enemyData.attackInterval;
        //enemyMoveType = enemyData.enemyMoveType;
        //point = enemyData.point;

    }

    protected virtual void Update() {

        // �G�l�~�[��Ώ�(�J����)�̕�����������
        if (lookTarget) {
            Vector3 direction = lookTarget.transform.position - transform.position;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }
    }

    protected virtual void OnTriggerStay(Collider other) {
        if (isAttack) {
            return;
        }

        // ���[�J���֐����`
        void SetAttackCoroutine() {
            // �U���p�̃��\�b�h�������ēo�^
            attackCoroutine = Attack(player);

            // �o�^�������\�b�h�����s
            StartCoroutine(attackCoroutine);

            Debug.Log("�U���J�n");

        }

        // �v���C���[�̏���ێ����Ă���A�U�����łȂ��Ȃ�
        if (player != null) {

            // �U���p�̃��\�b�h��o�^
            SetAttackCoroutine();

            Debug.Log("�v���C���[�@���m��");

            // �v���C���[�̏�񂪂Ȃ��Ȃ�
        } else {
            if (other.transform.parent.TryGetComponent(out player)) {

                // �U���p�̃��\�b�h��o�^
                SetAttackCoroutine();

                Debug.Log("�U���͈͓��Ƀv���C���[ �����m");
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) {

        // �v���C���[�����m�ς݂̂Ƃ��ɁA�U���͈͓��Ƀv���C���[�����Ȃ��Ȃ�����
        if (player != null) {

            // ������
            player = null;

            // �U���������~�߂�
            isAttack = false;
            StopCoroutine(attackCoroutine);

            Debug.Log("�U���͈͊O");
        }
    }

    /// <summary>
    /// �U��
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Attack(PlayerController player = null) {
        isAttack = true;

        player.CalcHp(-attackPower);

        if (anim) {
            anim.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(attackInterval);

        isAttack = false;
    }

    /// <summary>
    /// �U���͎擾�p
    /// </summary>
    /// <returns></returns>
    public int GetAttackPower() {
        return attackPower;
    }

    /// <summary>
    /// ���ۃN���X�̃��\�b�h������
    /// </summary>
    /// <param name="value"></param>
    /// <param name="hitBodyRegionType"></param>
    public override void TriggerEvent(int value, BodyRegionType hitBodyRegionType) {

        // �_���[�W�v�Z
        CalcDamage(value, hitBodyRegionType);
    }

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="hitParts"></param>
    public virtual void CalcDamage(int damage, BodyRegionType hitParts) {
        if (isDead) {
            return;
        }

        hp -= damage;

        if(anim) anim.ResetTrigger("Attack");

        if (hp <= 0) {
            isDead = true;

            if (anim) {
                anim.SetBool("Walk", false);
                anim.SetBool("Down", true);
            }

            // TODO �G�l�~�[�̏����O���N���X�� List �ŊǗ����Ă���ꍇ�ɂ́AList ����폜
            //gameManager.RemoveEnemyList(this);

            // ���ʂɂ�锻�肪����A���A����ł��ē|�����ꍇ
            if (hitParts == BodyRegionType.Head) {

                // ��������
                BodyRegionPartsController parts = partsControllersList.Find(x => x.GetBodyPartType() == hitParts);
                parts.gameObject.SetActive(false);

                // �X�R�A�Ƀ{�[�i�X(�C��)
                point *= 3;
            }

            // �X�R�A���Z
            

            Destroy(gameObject, 1.5f);
        } else {
            if(anim) anim.SetTrigger("Damage");
        }
    }

    /// <summary>
    /// �ړ����ꎞ��~
    /// </summary>
    public virtual void PauseMove() {
        
    }

    /// <summary>
    /// �ړ����ĊJ
    /// </summary>
    public virtual void ResumeMove() {
        
    }

    ///// <summary>
    ///// ���ʂ��Ƃ̏���ݒ�
    ///// </summary>
    //protected void SetBodyParts() {
    //    for (int i = 0; i < partsControllersList.Count; i++) {
    //        partsControllersList[i].SetUpPartsController(this);
    //    }
    //}
}
