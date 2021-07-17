using System.Collections;
using UnityEngine;

/// <summary>
/// Ray による弾の発射処理の制御クラス
/// </summary>
public class RayController : MonoBehaviour
{
    public Vector3 muzzleFlashScale;

    public GameObject muzzleFlashPrefab;

    public GameObject hitEffectPrefab;

    private bool isShooting;

    private GameObject muzzleFlashObj;
    private GameObject hitEffectObj;

    private GameObject target;
    private EnemyController enemy;

    [SerializeField, Header("Ray 用のレイヤー設定")]
    private int[] layerMasks;

    //[SerializeField]  Debug用
    private string[] layerMasksStr;

    private EventBase<int> eventBase;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField, HideInInspector]
    private ARManager arManager;

    [SerializeField, HideInInspector]
    private BodyRegionPartsController parts;

    void Start()
    {
        layerMasksStr = new string[layerMasks.Length];
        for (int i = 0; i < layerMasks.Length; i++) {
            layerMasksStr[i] = LayerMask.LayerToName(layerMasks[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ゲーム状態がプレイ中でない場合には処理を行わない制御をする
        //if (arManager.ARStateReactiveProperty.Value != ARState.Play) {
        //    return;
        //}

        // リロード判定(弾数 0 でリロード機能ありの場合)
        if (playerController.BulletCount == 0 && playerController.isReloadModeOn && Input.GetMouseButtonDown(0)) {
            StartCoroutine(playerController.ReloadBullet());
        }

        // 発射判定(弾数が残っており、リロード実行中でない場合)　押しっぱなしで発射できる
        if (playerController.BulletCount > 0  && !playerController.isReloading && Input.GetMouseButton(0)) {

            // 発射時間の計測
            StartCoroutine(ShootTimer());
        }
    }

    /// <summary>
    /// 継続的な弾の発射処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootTimer() {
        if (!isShooting) {
            isShooting = true;

            // 発射エフェクトの表示。初回のみ生成し、２回目はオンオフで切り替える
            if (muzzleFlashObj == null) {
                // 発射口の位置に RayController ゲームオブジェクトを配置する
                muzzleFlashObj = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
                muzzleFlashObj.transform.SetParent(gameObject.transform);
                muzzleFlashObj.transform.localScale = muzzleFlashScale;

            } else {
                muzzleFlashObj.SetActive(true);
            }

            // 発射
            Shoot();

            yield return new WaitForSeconds(playerController.shootInterval);

            muzzleFlashObj.SetActive(false);

            if (hitEffectObj != null) {
                hitEffectObj.SetActive(false);
            }

            isShooting = false;

        } else {
            yield return null;
        }


    }

    /// <summary>
    /// 弾の発射
    /// </summary>
    private void Shoot() {
        // カメラの位置から Ray を投射
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
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
                if (target.TryGetComponent(out eventBase)) {
                    eventBase.TriggerEvent(playerController.bulletPower);
                }

                // 演出
                PlayHitEffect(hit.point, hit.normal);

                //// ダメージ処理
                //if (target.TryGetComponent(out enemy)) {
                //    enemy.CalcDamage(playerController.bulletPower);

                //    // 演出
                //    PlayHitEffect(hit.point, hit.normal);
                //}
                //　同じ対象の場合
            } else {
                //if (target.TryGetComponent(out parts)) {
                //    parts.CalcDamageParts(playerController.bulletPower);
                //} else 
                if (target.TryGetComponent(out eventBase)) {
                    eventBase.TriggerEvent(playerController.bulletPower);                    
                }

                // 演出
                PlayHitEffect(hit.point, hit.normal);

                //if (target.TryGetComponent(out enemy)) {
                //    enemy.CalcDamage(playerController.bulletPower);

                //    // 演出
                //    PlayHitEffect(hit.point, hit.normal);
                //}
            }
        }

        playerController.CalcBulletCount(-1);
        //playerController.BulletCount--;
    }

    /// <summary>
    /// ヒット演出
    /// </summary>
    /// <param name="effectPos"></param>
    /// <param name="surfacePos"></param>
    private void PlayHitEffect(Vector3 effectPos, Vector3 surfacePos) {
        if (hitEffectObj == null) {
            hitEffectObj = Instantiate(hitEffectPrefab, effectPos, Quaternion.identity);
        } else {
            hitEffectObj.transform.position = effectPos;
            hitEffectObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, surfacePos);

            hitEffectObj.SetActive(true);
        }
    }
}
