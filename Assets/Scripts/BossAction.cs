using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossAction : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;

    public Transform[] moveTrans;

    public float bulletSpeed;

    public CapsuleCollider capsuleCollider;

    private Bullet bullet;

    public float waitInterval;

    private EnemyController_Normal enemyController;

    private Tween tween;

    public void MoveEnemy(float moveTime) {

        Sequence sequence = DOTween.Sequence();

        sequence.Append(enemyController.transform.DOLocalMove(moveTrans[0].localPosition, moveTime).SetEase(Ease.Linear));
        sequence.AppendInterval(1.0f);
        sequence.Append(enemyController.transform.DOLocalMove(moveTrans[1].localPosition, moveTime).SetEase(Ease.Linear));
        sequence.AppendInterval(1.0f);
        sequence.Append(enemyController.transform.DOLocalMove(moveTrans[2].localPosition, moveTime).SetEase(Ease.Linear));
        sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);

        tween = sequence;
    }

    /// <summary>
    /// íeÇÃê∂ê¨Ç∆î≠éÀ
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="enemyController"></param>
    public void GenerateBulletShot(Vector3 direction, int attackPower) {

        Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation).Shot(direction, bulletSpeed, attackPower);
        //bullet.SetUpBullet(attackPower);
        //bullet.damageArea.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);

        //Destroy(bullet.gameObject, 3.0f);

        Debug.Log("bullet genenrate End");
    }

    //private void OnTriggerEnter(Collider other) {

    //    Debug.Log("îªíË");

    //    if (other.gameObject.TryGetComponent(out PlayerController player)) {

    //        player.CalcHp(-enemyController.GetAttackPower());

    //        Debug.Log("ÉqÉbÉg");

    //        Destroy(bullet.gameObject);
    //    }
    //}

    /// <summary>
    /// ê›íË
    /// </summary>
    /// <param name="enemyController"></param>
    public void SetUpBossAction(EnemyController_Normal enemyController) {
        this.enemyController = enemyController;
    }
}
