using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// プレイヤー情報の管理クラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    private int hp;                // 現在のHp値
    private int bulletCount;       // 現在の弾数地

    [SerializeField, HideInInspector]
    private UIManager uiManagaer;

    [SerializeField, Header("最大Hp値")]
    private int maxHp;

    [SerializeField, Header("最大弾数値")]
    private int maxBullet;

    [SerializeField, Header("リロード時間")]
    private float reloadTime;

    [Header("弾の攻撃力")]
    public int bulletPower;

    [Header("弾の連射速度")]
    public float shootInterval;

    [Header("弾の射程距離")]
    public float shootRange;

    [Header("リロード機能のオン/オフ")]
    public bool isReloadModeOn;

    [Header("リロード状態の制御")]
    public bool isReloading;

    public int currentBulletNo;

    [System.Serializable]
    public struct CurrentBulletCount {
        public int bulletNo;
        public int bulletCount;
    }

    public List<CurrentBulletCount> currentBulletCountsList = new List<CurrentBulletCount>();


    /// <summary>
    /// 弾数用のプロパティ
    /// </summary>
    public int BulletCount
    {
        set => bulletCount = Mathf.Clamp(value, 0, maxBullet);
        get => bulletCount;        
    }

    //HP も同じようにプロパティ化できる


    void Start() {
        // Debug 用
        SetUpPlayer();
    }

    /// <summary>
    /// プレイヤー情報の初期設定
    /// </summary>
    public void SetUpPlayer() {

        // maxHp の設定があるか確認。なければ初期値 10 でセットして hp を設定
        hp = maxHp = maxHp == 0 ? 10 : maxHp;

        // maxBullet の設定があるか確認。なければ初期値 10 でセットして 弾数を設定
        BulletCount = maxBullet = maxBullet == 0 ? 10 : maxBullet;

        // 他の設定も初期値判定を作った方が安心

    }

    /// <summary>
    /// HPの計算と更新
    /// </summary>
    public void CalcHp(int amount) {
        hp = Mathf.Clamp(hp += amount, 0, maxHp);

        // HP 表示の更新
        //uiManagaer.UpdateDisplayLife(hp);

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
    }

    /// <summary>
    /// 弾数の計算と更新
    /// </summary>
    /// <param name="amout"></param>
    public void CalcBulletCount(int amout) {

        BulletCount += amout;

        Debug.Log("現在の弾数 : " + BulletCount);

        // 弾数のUI表示更新
        //uiManagaer.UpdateDisplayBulletCount(BulletCount);
    }

    /// <summary>
    /// 弾数のリロード
    /// </summary>
    public IEnumerator ReloadBullet() {

        // リロード状態にして、弾の発射を制御する
        isReloading = true;

        // リロード
        BulletCount = maxBullet;

        Debug.Log("リロード");

        // 弾数のUI表示更新
        //uiManagaer.UpdateDisplayBulletCount(BulletCount);

        // TODO SE

        // リロードの待機時間
        yield return new WaitForSeconds(reloadTime);

        // 再度、弾が発射できる状態にする
        isReloading = false;
    }

    private void OnTriggerEnter(Collider other) {

        // 敵からの攻撃によって被弾した場合の処理

        // ボスや敵の攻撃範囲を感知しないようにするためにタグでも判定するか、レイヤーを設定して回避する。ここではタグで管理
        if (other.gameObject.tag == "Bullet" && other.transform.parent.gameObject.TryGetComponent(out Bullet bullet)) {
            CalcHp(-bullet.attackPower);

            Destroy(other.gameObject);

            Debug.Log("敵の攻撃がプレイヤーにヒット");
        }
    }

    /// <summary>
    /// バレットの情報を更新
    /// </summary>
    /// <param name="weaponData"></param>
    public void ChangeBulletData(WeaponData weaponData) {

        // 初期武器以外の場合
        if (weaponData.weaponNo != 0) {

            // 弾が残っている場合で、まだ記録されていないとき
            if (bulletCount > 0 && !currentBulletCountsList.Exists(x => x.bulletNo == currentBulletNo)) {

                // 記録しておく
                currentBulletCountsList.Add(new CurrentBulletCount { bulletNo = currentBulletNo, bulletCount = bulletCount});
            }
        }

        bulletPower = weaponData.bulletPower;
        bulletCount = weaponData.maxBullet;

        reloadTime = weaponData.reloadTime;
        shootInterval = weaponData.shootInterval;
        shootRange = weaponData.shootRange;

        // 記録されている弾数がある場合には、そちらにする
        bulletCount = currentBulletCountsList.Find(x => x.bulletNo == weaponData.weaponNo).bulletCount;

    }
}
