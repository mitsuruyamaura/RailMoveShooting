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

    [SerializeField]
    private int bulletCount;       // 現在の弾数

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

    [SerializeField]
    private UIManager uiManagaer;

    public int currentWeaponNo;

    [System.Serializable]
    public class BulletCountData {
        public int bulletNo;
        public int bulletCount;

        /// <summary>
        /// 残弾数の更新
        /// </summary>
        /// <param name="amount"></param>
        public void SetBulletCount(int amount) {
            bulletCount = amount;
            Debug.Log("残弾数更新");
        }
    }

    public List<BulletCountData> bulletCountDatasList = new List<BulletCountData>();


    /// <summary>
    /// 弾数用のプロパティ
    /// </summary>
    public int BulletCount
    {
        set => bulletCount = Mathf.Clamp(value, 0, maxBullet);
        get => bulletCount;
    }

    //HP も同じようにプロパティ化できる


    // mi

    private bool isShootPermission;

    public bool IsShootPerimission
    {
        set => isShootPermission = value;
        get => isShootPermission;
    }

    [SerializeField]
    private GameObject playerObj;

    [SerializeField]
    private GameObject weaponObj;


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
        uiManagaer.SetPlayerInfo(maxHp, BulletCount);


        // mi
        IsShootPerimission = true;
         
    }

    /// <summary>
    /// HPの計算と更新
    /// </summary>
    public void CalcHp(int amount) {
        hp = Mathf.Clamp(hp += amount, 0, maxHp);

        // HP 表示の更新
        uiManagaer.UpdateDisplayLife(hp);

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
    /// 武器(バレット)の情報を変更して武器切り替え
    /// </summary>
    /// <param name="weaponData"></param>
    public void ChangeBulletData(WeaponData weaponData) {

        // 現在使用している武器の番号を保持
        currentWeaponNo = weaponData.weaponNo;

        // 武器の情報を各変数に設定
        bulletPower = weaponData.bulletPower;
        maxBullet = weaponData.maxBullet;

        reloadTime = weaponData.reloadTime;
        shootInterval = weaponData.shootInterval;
        shootRange = weaponData.shootRange;

        bulletCount = maxBullet;

        // すでに使用したことのある武器である場合
        if (bulletCountDatasList.Exists(x =>  x.bulletNo == currentWeaponNo)) {

            // 弾数を前回の残弾数にする
            bulletCount = bulletCountDatasList.Find(x => x.bulletNo == currentWeaponNo).bulletCount;
        }
    }

    /// <summary>
    /// 使用している武器のデータ(武器の番号と残弾数)を記録して更新
    /// </summary>
    /// <param name="weaponData"></param>
    public void UpdateCurrentBulletCountData(WeaponData weaponData) {

        // まだ一度も使用している武器の残弾数を記録していないとき
        if (!bulletCountDatasList.Exists(x => x.bulletNo == currentWeaponNo)) {
            // 新しくデータを作成して、記録
            bulletCountDatasList.Add(new BulletCountData { bulletNo = currentWeaponNo, bulletCount = bulletCount });
        } else {
            // 使用したことがある武器ですでにデータがある場合には、そのデータを見つけて上書きして記録
            bulletCountDatasList.Find(x => x.bulletNo == currentWeaponNo).SetBulletCount(bulletCount);
        }

        // 武器の情報を設定
        ChangeBulletData(weaponData);
    }

    /// <summary>
    /// ゲームクリア時の演出の準備
    /// </summary>
    public void PrepareClearSettings() {
        // プレイヤーのゲームオブジェクトとの親子関係の解消
        playerObj.transform.SetParent(null);
        // 武器を非表示
        weaponObj.SetActive(false);
    }
}
