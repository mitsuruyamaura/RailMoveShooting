using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject lookTarget;

    [SerializeField]
    private int enemyNo;

    [SerializeField]
    private int hp;

    [SerializeField]
    private int attackPower;

    [SerializeField]
    private float moveSpeed;

    private Animator anim;

    private bool isAttack;

    private float attackInterval = 3.0f;

    private PlayerController player;

    private GameManager gameManager;

    private IEnumerator attackCoroutine;

    private bool isDead;

    private NavMeshAgent agent;

    private float originMoveSpeed;  // �����̈ړ����x�̕ێ��p

    public EnemyMoveType enemyMoveType;


    /// <summary>
    /// �G�l�~�[�̏����ݒ�
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="gameManager"></param>
    public void SetUpEnemy(PlayerController playerController, GameManager gameManager) {

        lookTarget = playerController.gameObject;
        this.gameManager = gameManager;

        TryGetComponent(out anim);

        // NavMesh �𗘗p���Ă��邩����
        if (TryGetComponent(out agent)) {

            // ���p���Ă���ꍇ�ɂ́A�������x���L�^
            originMoveSpeed = moveSpeed;

            // �ڕW�n�_���Z�b�g
            agent.destination = lookTarget.transform.position;

            // �ړ����x�� NavMesh �ɐݒ�
            agent.speed = moveSpeed;

            // �A�j��������ꍇ�ɂ͍Đ�
            if (anim) {
                anim.SetBool("Walk", true);
            }
        }

        Debug.Log("�G�l�~�[�̐ݒ芮��");

        SoundManager.instance.PlaySE(SoundManager.SE_Type.Zombie_1_Enter);
    }

    void Update() {

        // �G�l�~�[��Ώ�(�J����)�̕�����������
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

    private void OnTriggerStay(Collider other) {
        if (isAttack) {
            return;
        }

        if (other.tag != "MainCamera") {   // �n�ʂ̃R���C�_�[��A���̃G�l�~�[�̃R���C�_�[���E���Ă��܂�����
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

                Debug.Log("�U���͈͓��Ƀv���C���[ �����m");
            }
        }

        // ���[�J���֐����`
        void SetAttackCoroutine() {
            // �U���p�̃��\�b�h�������ēo�^
            attackCoroutine = Attack(player);

            // �o�^�������\�b�h�����s
            StartCoroutine(attackCoroutine);

            Debug.Log("�U���J�n");
        }
    }

    private void OnTriggerExit(Collider other) {

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
    private IEnumerator Attack(PlayerController player = null) {
        isAttack = true;

        SoundManager.instance.PlaySE(SoundManager.SE_Type.Zombie_1_Attack);

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
    /// �v���C���[����U�����󂯂��ۂɌĂ΂�鏈��
    /// </summary>
    /// <param name="damage"></param>
    public void TriggerEvent(int damage) {

        // �_���[�W�v�Z
        CalcDamage(damage);
    }

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="damage"></param>
    private void CalcDamage(int damage) {
        if (isDead) {
            return;
        }

        hp -= damage;

        // �A�j���[�V�����̐ݒ肪����ꍇ
        if (anim) anim.ResetTrigger("Attack");

        if (hp <= 0) {
            isDead = true;

            SoundManager.instance.PlaySE(SoundManager.SE_Type.Zombie_1_Down);

            if (anim) {
                anim.SetBool("Walk", false);
                anim.SetBool("Down", true);
            }

            // �G�l�~�[�̏����O���N���X�� List �ŊǗ����Ă���ꍇ�ɂ́AList ����폜
            gameManager.RemoveEnemyList(this);

            Destroy(gameObject, 1.5f);
        } else {

            // �A�j���[�V�����̐ݒ肪����ꍇ
            if (anim) anim.SetTrigger("Damage");
        }
    }

    /// <summary>
    /// �ړ����ꎞ��~
    /// </summary>
    public void PauseMove() {
        agent.speed = 0;
    }

    /// <summary>
    /// �ړ����ĊJ
    /// </summary>
    public void ResumeMove() {
        agent.speed = originMoveSpeed;
    }

    /// <summary>
    /// �ړI�n������
    /// </summary>
    public void ClearPath() {
        agent.ResetPath();
    }
}
