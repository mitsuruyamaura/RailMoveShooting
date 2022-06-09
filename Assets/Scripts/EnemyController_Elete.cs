using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Elete : EnemyBase
{
    public float waitTime;

    public override void SetUpEvent(PlayerController playerController, GameManager gameManager) {
        base.SetUpEvent(playerController, gameManager);

        StartCoroutine(AttackRegularIntervals());
    }

    /// <summary>
    /// ���Ԋu���ƂɊm��U��
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackRegularIntervals() {

        float timer = 0;

        while (true) {
            timer += Time.deltaTime;
            if (timer > waitTime) {
                timer = 0;
                // �U���p�̃��\�b�h�������ēo�^
                attackCoroutine = Attack(player);
                StartCoroutine(attackCoroutine);
            }
            yield return null;
        }
    }
}
