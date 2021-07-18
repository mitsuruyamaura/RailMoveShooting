using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public enum EnemyMoveType {
    Agent,
    Boss_0,
    Boss_1
}

public class EnemyBase : MonoBehaviour  //  EventBase<int>    
{
    protected Animator anim;
    protected Tween tween;

    protected GameObject lookTarget;

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


    public virtual void SetUpEnemy(GameObject playerObj, GameManager gameManager) {

        lookTarget = playerObj;
        this.gameManager = gameManager;

        // �G�̃f�[�^��G�̔ԍ����猟�����ăZ�b�g
        GetEnemyData();

        TryGetComponent(out anim);

        // NavMesh �𗘗p���Ă��邩����
        if (TryGetComponent(out agent)) {

            // ���p���Ă���ꍇ�ɂ͖ڕW�n�_���Z�b�g
            agent.destination = lookTarget.transform.position;

            // �ړ����x�� NavMesh �ɐݒ�
            if (anim) {
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
        if (player != null && !isAttack) {

            // �U���p�̃��\�b�h��o�^
            SetAttackCoroutine();

            Debug.Log("�v���C���[�@���m��");

            // �v���C���[�̏�񂪂Ȃ��Ȃ�
        } else {
            if (other.gameObject.TryGetComponent(out player)) {

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

    //public override void TriggerEvent(int value) {
    //    CalcDamage(value);
    //}

    ///// <summary>
    ///// �_���[�W�v�Z
    ///// </summary>
    ///// <param name="damage"></param>
    //public void CalcDamage(int damage, BodyRegionType bodyPartType = BodyRegionType.Boby) {
    //    if (isDead) {
    //        return;
    //    }

    //}
}
