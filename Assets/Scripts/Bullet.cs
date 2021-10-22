using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    //public float shotSpeed;

    public GameObject damageArea;

    public int attackPower;

    /// <summary>
    /// 弾の設定と発射
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="bulletSpeed"></param>
    /// <param name="attackPower"></param>
    public void Shot(Vector3 direction, float bulletSpeed, int attackPower) {
        this.attackPower = attackPower;

        damageArea.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);

        Destroy(gameObject, 3.0f);
    }
}
