using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

/// <summary>
/// �G�l�~�[�̈ړ����@�̎��
/// </summary>
public enum EnemyMoveType {
    Agent,
    Boss_0,
    Boss_1
}

/// <summary>
/// �G�l�~�[�̊�N���X
/// </summary>
public class EnemyBase : EventBase<int>  //  EventBase<int>    
{
    protected Animator anim;
    protected Tween tween;

    [SerializeField]
    protected GameObject lookTarget;

    [SerializeField]
    protected int enemyNo;

    [SerializeField]
    protected int hp;

    [SerializeField]
    protected int attackPower;

    protected NavMeshAgent agent;

    protected bool isAttack;

    protected float attackInterval = 3.0f;

    protected PlayerController player;

    protected GameManager gameManager;

    protected IEnumerator attackCoroutine;

    protected int point = 100;

    protected bool isDead;

    public EnemyMoveType enemyMoveType;

    // �G�̃f�[�^����������


    [SerializeField]
    protected List<BodyRegionPartsController> partsControllersList = new List<BodyRegionPartsController>();


    void Start() {
        // �f�o�b�O�p
        SetUpEnemy(lookTarget);
    }

    public virtual void SetUpEnemy(GameObject playerObj, GameManager gameManager = null) {

        lookTarget = playerObj;
        this.gameManager = gameManager;

        // �G�̃f�[�^��G�̔ԍ����猟�����ăZ�b�g
        GetEnemyData();

        TryGetComponent(out anim);

        // NavMesh �𗘗p���Ă��邩����
        if (TryGetComponent(out agent)) {

            // ���p���Ă���ꍇ�ɂ͖ڕW�n�_���Z�b�g
            agent.destination = lookTarget.transform.position;
                  
            // �A�j��������ꍇ�ɂ͍Đ�
            if (anim) {

                // �ړ����x�� NavMesh �ɐݒ�

                anim.SetBool("Walk", true);
            }
        }

        // ���ʂ��Ƃ̏���ݒ�
        for (int i = 0; i < partsControllersList.Count; i++) {
            //partsControllersList[i].SetUpPartsController(this);
        }
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

        // �G��Ώ�(�J����)�̕�����������
        if (lookTarget) {
            Vector3 direction = lookTarget.transform.position - transform.position;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }

        // �ړI�n���X�V
        if (lookTarget != null && agent != null) {
            agent.destination = lookTarget.transform.position;
        }
    }

    protected virtual void OnTriggerStay(Collider other) {
        if (isAttack) {
            return;
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

                Debug.Log("�v���C���[ �����m");
            }
        }

        // �U���p�̃��\�b�h��o�^
        void SetAttackCoroutine() {
            // �U���p�̃��\�b�h�������ēo�^
            attackCoroutine = Attack(player);

            // �o�^�������\�b�h�����s
            StartCoroutine(attackCoroutine);

            Debug.Log("�U���J�n");
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

            Debug.Log("�͈͊O");
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

    public int GetAttackPower() {
        return attackPower;
    }

    public override void TriggerEvent(int value) {
        CalcDamage(value);
    }

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="damage"></param>
    public virtual void CalcDamage(int damage, BodyRegionType bodyPartType = BodyRegionType.Boby) {
        if (isDead) {
            return;
        }

    }
}
