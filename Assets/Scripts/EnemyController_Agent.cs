using UnityEngine;
using UnityEngine.AI;

public class EnemyController_Agent : EnemyBase
{
    protected NavMeshAgent agent;

    private float originMoveSpeed;  // �����̈ړ����x�̕ێ��p

    /// <summary>
    /// �G�l�~�[�̐ݒ�
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="gameManager"></param>
    public override void SetUpEvent(PlayerController playerController, GameManager gameManager) {
        base.SetUpEvent(playerController, gameManager);

        // NavMesh �𗘗p���Ă��邩����
        if (TryGetComponent(out agent)) {
            originMoveSpeed = moveSpeed;

            // ���p���Ă���ꍇ�ɂ͖ڕW�n�_���Z�b�g
            agent.destination = lookTarget.transform.position;

            // �ړ����x�� NavMesh �ɐݒ�
            agent.speed = moveSpeed;

            // �A�j��������ꍇ�ɂ͍Đ�
            if (anim) {
                anim.SetBool("Walk", true);
            }
        }
    }

    protected override void Update() {
        base.Update();

        // �ړI�n���X�V
        if (lookTarget != null && agent != null) {
            agent.destination = lookTarget.transform.position;
        }
    }

    /// <summary>
    /// �ړ����ꎞ��~
    /// </summary>
    public override void PauseMove() {
        agent.speed = 0;
    }

    /// <summary>
    /// �ړ����ĊJ
    /// </summary>
    public override void ResumeMove() {
        agent.speed = originMoveSpeed;
    }

    /// <summary>
    /// �ړI�n������
    /// </summary>
    public void ClearPath() {
        agent.ResetPath();
    }
}
