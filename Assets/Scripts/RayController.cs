using System.Collections;
using UnityEngine;

/// <summary>
/// Ray �ɂ��e�̔��ˏ����̐���N���X
/// </summary>
public class RayController : MonoBehaviour
{
    [Header("���ˌ��p�̃G�t�F�N�g�̃T�C�Y����")]
    public Vector3 muzzleFlashScale;

    private bool isShooting;

    private GameObject muzzleFlashObj;
    private GameObject hitEffectObj;

    private GameObject target;
    private EnemyController enemy;

    [SerializeField, Header("Ray �p�̃��C���[�ݒ�")]
    private int[] layerMasks;

    //[SerializeField]  Debug�p
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

    void Update()
    {
        // �Q�[����Ԃ��v���C���łȂ��ꍇ�ɂ͏������s��Ȃ����������
        //if (arManager.ARStateReactiveProperty.Value != ARState.Play) {
        //    return;
        //}

        // �����[�h����(�e�� 0 �Ń����[�h�@�\����̏ꍇ)
        if (playerController.BulletCount == 0 && playerController.isReloadModeOn && Input.GetMouseButtonDown(0)) {
            StartCoroutine(playerController.ReloadBullet());
        }

        // ���˔���(�e�����c���Ă���A�����[�h���s���łȂ��ꍇ)�@�������ςȂ��Ŕ��˂ł���
        if (playerController.BulletCount > 0  && !playerController.isReloading && Input.GetMouseButton(0)) {

            // ���ˎ��Ԃ̌v��
            StartCoroutine(ShootTimer());
        }
    }

    /// <summary>
    /// �p���I�Ȓe�̔��ˏ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootTimer() {
        if (!isShooting) {
            isShooting = true;

            // ���˃G�t�F�N�g�̕\���B����̂ݐ������A�Q��ڂ̓I���I�t�Ő؂�ւ���
            if (muzzleFlashObj == null) {
                // ���ˌ��̈ʒu�� RayController �Q�[���I�u�W�F�N�g��z�u����
                muzzleFlashObj = Instantiate(EffectManager.instance.muzzleFlashPrefab, transform.position, transform.rotation);
                muzzleFlashObj.transform.SetParent(gameObject.transform);
                muzzleFlashObj.transform.localScale = muzzleFlashScale;

            } else {
                muzzleFlashObj.SetActive(true);
            }

            // ����
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
    /// �e�̔���
    /// </summary>
    private void Shoot() {
        // �J�����̈ʒu���� Ray �𓊎�
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, playerController.shootRange, LayerMask.GetMask(layerMasksStr))) {

            Debug.Log(hit.collider.gameObject.name);

            // �����Ώۂ��U�����Ă��邩�m�F�B�Ώۂ����Ȃ��������A�����ΏۂłȂ��ꍇ
            if (target == null || target != hit.collider.gameObject) {

                // �N���X���p�����Ďg���悤�ɂ��āATryGetComponent �̏����� Base ���擾���ē��ꂷ��
                target = hit.collider.gameObject;

                //if (target.TryGetComponent(out parts)) {
                //    parts.CalcDamageParts(playerController.bulletPower);
                //}
                //else 
                if (target.TryGetComponent(out eventBase)) {
                    eventBase.TriggerEvent(playerController.bulletPower);
                }

                // ���o
                PlayHitEffect(hit.point, hit.normal);

                //// �_���[�W����
                //if (target.TryGetComponent(out enemy)) {
                //    enemy.CalcDamage(playerController.bulletPower);

                //    // ���o
                //    PlayHitEffect(hit.point, hit.normal);
                //}
                //�@�����Ώۂ̏ꍇ
            } else {
                //if (target.TryGetComponent(out parts)) {
                //    parts.CalcDamageParts(playerController.bulletPower);
                //} else 
                if (target.TryGetComponent(out eventBase)) {
                    eventBase.TriggerEvent(playerController.bulletPower);                    
                }

                // ���o
                PlayHitEffect(hit.point, hit.normal);

                //if (target.TryGetComponent(out enemy)) {
                //    enemy.CalcDamage(playerController.bulletPower);

                //    // ���o
                //    PlayHitEffect(hit.point, hit.normal);
                //}
            }
        }

        playerController.CalcBulletCount(-1);
    }

    /// <summary>
    /// �q�b�g���o
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
}
