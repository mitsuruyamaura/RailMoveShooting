using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// Ray による弾の発射処理の制御クラス
/// </summary>
public class RayController : MonoBehaviour
{
    [Header("発射口用のエフェクトのサイズ調整")]
    public Vector3 muzzleFlashScale;

    private bool isShooting;

    private GameObject muzzleFlashObj;
    private GameObject hitEffectObj;

    private GameObject target;
    private EnemyController enemy;

    [SerializeField, Header("Ray 用のレイヤー設定")]
    private int[] layerMasks;

    [SerializeField]  //Debug用
    private string[] layerMasksStr;

    [SerializeField]
    private PlayerController playerController;

    private EventBase eventBase;

    //[SerializeField, HideInInspector]
    //private BodyRegionPartsController parts;

    [SerializeField]
    private List<EnemyController> targetList = new List<EnemyController>();

    [SerializeField]
    private List<EventBase> lockonEventList = new List<EventBase>();

    [SerializeField]
    private List<TargetMarker> markerList = new List<TargetMarker>();

    private List<RaycastHit> hitList = new List<RaycastHit>();

    [SerializeField]
    private TargetMarker targetMarkerPrefab;


    // mi

    [SerializeField, HideInInspector]
    private ARManager arManager;


    void Start() {
        layerMasksStr = new string[layerMasks.Length];
        for (int i = 0; i < layerMasks.Length; i++) {
            layerMasksStr[i] = LayerMask.LayerToName(layerMasks[i]);
        }

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => playerController.IsShootPerimission)
            .Where(_ => playerController.BulletCount > 0 && !playerController.isReloading)
            .ThrottleFirst(TimeSpan.FromSeconds(playerController.shootInterval))
            .Subscribe(_ => {

                // ロックオン
                if (Input.GetButton("Fire2")) {
                    LockOnTargets();
                    //Debug.Log("Lock on");                    
                }

                if (Input.GetMouseButton(0)) {
                    StartCoroutine(ShootTimer());
                }
            });
    }

    /// <summary>
    /// ロックオン
    /// </summary>
    private void LockOnTargets() {

        // 弾数以上はロックオンできない
        if (playerController.BulletCount <= targetList.Count) {
            return;
        }

        // カメラの位置から正面に向かって Ray を投射
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // クリックした位置用
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction, Color.green, 3.0f);

        if (Physics.Raycast(ray, out RaycastHit hit, playerController.shootRange, LayerMask.GetMask(layerMasksStr))) {

            //Debug.Log(hit.collider.gameObject.name);

            // Ray の Hit した対象が Event であり、かつ、ターゲットのリストに登録されていない場合
            if (hit.collider.TryGetComponent(out EventBase eventBase) && !lockonEventList.Contains(eventBase)) {
                lockonEventList.Add(eventBase);

            }
            else if (hit.collider.TryGetComponent(out EnemyController enemy) && !targetList.Contains(enemy)) {

                targetList.Add(enemy);
            }
            
            // 対象に照準マーカーを付与
            TargetMarker targetMarker = Instantiate(targetMarkerPrefab, hit.collider.gameObject.transform);
            targetMarker.SetUpTargetMarker(playerController.transform);
            markerList.Add(targetMarker);

            // ヒットした地点の登録
            hitList.Add(hit);
        }
    }

    /// <summary>
    /// ロックオンした対象に順番に発射
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootLockOnTargets() {

        for (int i = 0; i < targetList.Count; i++) {

            // 発射エフェクトの表示。初回のみ生成し、２回目はオンオフで切り替える
            if (muzzleFlashObj == null) {
                // 発射口の位置に RayController ゲームオブジェクトを配置する
                muzzleFlashObj = Instantiate(EffectManager.instance.muzzleFlashPrefab, transform.position, transform.rotation);
                muzzleFlashObj.transform.SetParent(gameObject.transform);
                muzzleFlashObj.transform.localScale = muzzleFlashScale;

            } else {
                muzzleFlashObj.SetActive(true);
            }

            // ダメージ処理(デバッグ用。教材用)
            targetList[i].TriggerEvent(playerController.bulletPower);

            // 演出
            PlayHitEffect(hitList[i].point, hitList[i].normal);

            // マーカー削除
            Destroy(markerList[i].gameObject);

            // SE
            SoundManager.instance.PlaySE(SoundManager.SE_Type.Gun_1);

            playerController.CalcBulletCount(-1);

            yield return new WaitForSeconds(playerController.shootInterval);

            muzzleFlashObj.SetActive(false);

            if (hitEffectObj != null) {
                hitEffectObj.SetActive(false);
            }
        }
        targetList.Clear();
        markerList.Clear();
        hitList.Clear();
    }

    void Update()
    {
        // ゲーム状態がプレイ中でない場合には処理を行わない制御をする
        //if (arManager.ARStateReactiveProperty.Value != ARState.Play) {
        //    return;
        //}


#if UNITY_EDITOR
        // UI がタップされたときは処理しない(UI のボタンを押したらそちらのみを反応させる)
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
#else   // スマホ用
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return;
        }
#endif

        // 発射許可がない場合
        if (!playerController.IsShootPerimission) {
            // 処理しない
            return;
        }

        // リロード判定(弾数 0 でリロード機能ありの場合)
        if (playerController.BulletCount == 0 && playerController.isReloadModeOn && Input.GetMouseButtonDown(0)) {
            StartCoroutine(playerController.ReloadBullet());
        }

        // ロックオン対象がいる場合には各ロックオン対象に自動発射
        if (Input.GetButtonUp("Fire2") && targetList.Count > 0) {
            StartCoroutine(ShootLockOnTargets());
            Debug.Log("Lock on Fire");
        }

        // 発射判定(弾数が残っており、リロード実行中でない場合)　押しっぱなしで発射できる
        //if (playerController.BulletCount > 0  && !playerController.isReloading && Input.GetMouseButton(0)) {

        //    // 発射時間の計測
        //    StartCoroutine(ShootTimer());
        //}
    }

    /// <summary>
    /// 継続的な弾の発射処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootTimer() {
        //if (!isShooting) {
        //    isShooting = true;

        // 発射エフェクトの表示。初回のみ生成し、２回目はオンオフで切り替える
        if (muzzleFlashObj == null) {
            // 発射口の位置に RayController ゲームオブジェクトを配置する
            muzzleFlashObj = Instantiate(EffectManager.instance.muzzleFlashPrefab, transform.position, transform.rotation);
            muzzleFlashObj.transform.SetParent(gameObject.transform);
            muzzleFlashObj.transform.localScale = muzzleFlashScale;

        } else {
            muzzleFlashObj.SetActive(true);
        }

        // 発射
        Shoot();

        // SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Gun_1);

        yield return new WaitForSeconds(playerController.shootInterval);

        muzzleFlashObj.SetActive(false);

        if (hitEffectObj != null) {
            hitEffectObj.SetActive(false);
        }

        //    isShooting = false;

        //} else {
        //    yield return null;
        //}
    }

    /// <summary>
    /// 弾の発射
    /// </summary>
    private void Shoot() {
        // カメラの位置から正面に向かって Ray を投射
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // クリックした位置用
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, playerController.shootRange, LayerMask.GetMask(layerMasksStr))) {

            Debug.Log(hit.collider.gameObject.name);

            // 同じ対象を攻撃しているか確認。対象がいなかったか、同じ対象でない場合
            if (target == null || target != hit.collider.gameObject) {

                // クラスを継承して使うようにして、TryGetComponent の処理を Base を取得して統一する
                target = hit.collider.gameObject;

                //if (target.TryGetComponent(out parts)) {
                //    parts.CalcDamageParts(playerController.bulletPower);
                //}
                //else 

                // 通常用
                //if (target.TryGetComponent(out eventBase)) {

                //    // 部位によるダメージ量の修正を計算 
                //    CalcDamage();

                //    // 演出
                //    PlayHitEffect(hit.point, hit.normal);
                //}

                // ダメージ処理(デバッグ用。教材用)
                if (target.TryGetComponent(out enemy)) {
                    enemy.TriggerEvent(playerController.bulletPower);

                    // 演出
                    PlayHitEffect(hit.point, hit.normal);
                }
                
            //　同じ対象の場合
            } else if (target == hit.collider.gameObject) {
                //if (target.TryGetComponent(out parts)) {
                //    parts.CalcDamageParts(playerController.bulletPower);
                //} else 
                //if (target.TryGetComponent(out eventBase)) {

                // TODO 本番用
                // 通常。部位によるダメージ量の修正を計算 
                //CalcDamage();

                //eventBase.TriggerEvent(playerController.bulletPower);
                //}

                // 演出
                //PlayHitEffect(hit.point, hit.normal);


                // デバッグ用
                if (target.TryGetComponent(out enemy)) {
                    enemy.TriggerEvent(playerController.bulletPower);

                    // 演出
                    PlayHitEffect(hit.point, hit.normal);
                }
            }
        }

        playerController.CalcBulletCount(-1);
    }

    /// <summary>
    /// ヒット演出
    /// </summary>
    /// <param name="effectPos"></param>
    /// <param name="surfacePos"></param>
    private void PlayHitEffect(Vector3 effectPos, Vector3 surfacePos) {
        if (hitEffectObj == null) {
            hitEffectObj = Instantiate(EffectManager.instance.hitEffectPrefab, effectPos, Quaternion.identity);
        } else {
            hitEffectObj.transform.position = effectPos;
            hitEffectObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, surfacePos);

            hitEffectObj.SetActive(true);
        }
    }

    /// <summary>
    /// 部位によるダメージ量の修正を計算
    /// </summary>
    /// <returns></returns>
    private void CalcDamage() {
        (int lastDamage, BodyRegionType hitRegionType) partsValue;

        if (target.TryGetComponent(out BodyRegionPartsController parts)) {
            partsValue = parts.CalcDamageParts(playerController.bulletPower);
        } else {
            partsValue = (playerController.bulletPower, BodyRegionType.Boby);
        }

        // 部位とダメージ決定
        eventBase.TriggerEvent(partsValue.lastDamage, partsValue.hitRegionType);
    }
}
