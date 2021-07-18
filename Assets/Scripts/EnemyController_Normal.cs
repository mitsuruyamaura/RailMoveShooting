using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System;
using System.Linq;

public class EnemyController_Normal : EnemyBase
{
    //[SerializeField]
    //private BossAction bossAction;


    //public void MoveEnemy(float moveTime) {
    //    //anim = GetComponent<Animator>();
    //    //anim.SetTrigger("jump");
    //    //tween = transform.DOMoveY(2.5f, 3.0f).SetEase(Ease.InBack)
    //    //    .OnComplete(() => {
    //    //        transform.DOMoveY(0, 3.0f).SetEase(Ease.InBack)
    //    //            .OnComplete(() => {
    //    //                Destroy(gameObject);
    //    //            });
    //    //    });

    //    //tween = transform.DOMove(lookTarget.transform.position, 5.0f)
    //    //    .SetEase(Ease.Linear)
    //    //    .OnComplete(() => { Destroy(gameObject); });

    //    Sequence sequence = DOTween.Sequence();

    //    sequence.Append(transform.DOLocalMove(bossAction.moveTrans[0].localPosition, moveTime).SetEase(Ease.Linear));
    //    sequence.AppendInterval(1.0f);
    //    sequence.Append(transform.DOLocalMove(bossAction.moveTrans[1].localPosition, moveTime).SetEase(Ease.Linear));
    //    sequence.AppendInterval(1.0f);
    //    sequence.Append(transform.DOLocalMove(bossAction.moveTrans[2].localPosition, moveTime).SetEase(Ease.Linear));
    //    sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);

    //    tween = sequence;
    //}

    /// <summary>
    /// �ړ����ꎞ��~
    /// </summary>
    public void PauseMove() {
        tween.Pause();
    }

    /// <summary>
    /// �ړ����ĊJ
    /// </summary>
    public void ResumeMove() {
        tween.Play();
    }

    ///// <summary>
    ///// �G�̐ݒ�
    ///// </summary>
    ///// <param name="playerObj"></param>
    ///// <param name="gameManager"></param>
    ///// <returns></returns>
    //public IEnumerator SetUpEnemyController(GameObject playerObj, GameManager gameManager) {
    //    lookTarget = playerObj;
    //    this.gameManager = gameManager;

    //    SetUpEnemyData();

    //    //TryGetComponent(out agent);
    //    TryGetComponent(out anim);

    //    // TODO Type �ŕ��򂵁A Agent �̎��ɂ� AddComponet ����

    //    //agent = gameObject.AddComponent<NavMeshAgent>();

    //    if (TryGetComponent(out agent)) {
    //        agent.destination = lookTarget.transform.position;
    //    }

    //    for (int i = 0; i < partsControllersList.Count; i++) {
    //        partsControllersList[i].SetUpPartsController(this);
    //    }

    //    if (enemyMoveType == EnemyMoveType.Agent) {

    //        agent.speed = enemyData.moveValue;
    //        anim.SetBool("Walk", true);
    //    }

    //    if (enemyMoveType == EnemyMoveType.Boss_0) {
    //        bossAction.SetUpBossAction(this);
    //        bossAction.MoveEnemy(enemyData.moveValue);

    //    }
    //    //MoveEnemy();

    //    yield return null;
    //}

    //private void OnTriggerEnter(Collider other) {
    //    if (other.CompareTag("Bullet")) {
    //        tween.Kill();

    //        anim.ResetTrigger("jump");
    //        anim.SetTrigger("stun");

    //        Destroy(other.gameObject);

    //        Destroy(gameObject, 1.0f);
    //    }
    //}

    ///// <summary>
    ///// �_���[�W�v�Z
    ///// </summary>
    ///// <param name="damage"></param>
    //public void CalcDamage(int damage, BodyRegionType bodyPartType = BodyRegionType.Boby) {
    //    if (isDead) {
    //        return;
    //    }

    //    hp -= damage;

    //    anim.ResetTrigger("Attack");

    //    if (hp <= 0) {
    //        isDead = true;

    //        anim.SetBool("Walk", false);

    //        anim.SetBool("Down", true);

    //        gameManager.RemoveEnemyList(this);       

    //        // ����ł��ē|�����ꍇ
    //        if (bodyPartType == BodyRegionType.Head) {

    //            // ��������
    //            BodyRegionPartsController parts = partsControllersList.Find(x => x.GetBodyPartType() == bodyPartType);
    //            parts.gameObject.SetActive(false);

    //            point *= 3;
    //        }

    //        // �X�R�A���Z
    //        GameData.instance.scoreReactiveProperty.Value += point;

    //        Destroy(gameObject, 1.5f);
    //    } else {
    //        anim.SetTrigger("Damage");
    //    }
    //}

    //public override void TriggerMission(int value) {
    //    CalcDamage(value);
    //}

    //private IEnumerator AttackBoss_0() {
    //    isAttack = true;

    //    anim.SetTrigger("Attack");

    //    bossAction.capsuleCollider.enabled = false;

    //    yield return new WaitForSeconds(bossAction.waitInterval);

    //    bossAction.GenerateBulletShot(player.transform.position - transform.position, attackPower);

    //    yield return new WaitForSeconds(attackInterval);

    //    bossAction.capsuleCollider.enabled = true;

    //    isAttack = false;
    //}
}
