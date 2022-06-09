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
    /// ˆê’èŠÔŠu‚²‚Æ‚ÉŠm’èUŒ‚
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackRegularIntervals() {

        float timer = 0;

        while (true) {
            timer += Time.deltaTime;
            if (timer > waitTime) {
                timer = 0;
                // UŒ‚—p‚Ìƒƒ\ƒbƒh‚ğ‘ã“ü‚µ‚Ä“o˜^
                attackCoroutine = Attack(player);
                StartCoroutine(attackCoroutine);
            }
            yield return null;
        }
    }
}
