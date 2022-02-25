using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.EventSystems;

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

    [SerializeField]  //Debug�p
    private string[] layerMasksStr;

    [SerializeField]
    private PlayerController playerController;

    private EventBase eventBase;

    //[SerializeField, HideInInspector]
    //private BodyRegionPartsController parts;


    // mi

    [SerializeField, HideInInspector]
    private ARManager arManager;




    void Start()
    {
        layerMasksStr = new string[layerMasks.Length];
        for (int i = 0; i < layerMasks.Length; i++) {
            layerMasksStr[i] = LayerMask.LayerToName(layerMasks[i]);
        }

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => playerController.IsShootPerimission)
            .Where(_ => playerController.BulletCount > 0 && !playerController.isReloading && Input.GetMouseButton(0))
            .ThrottleFirst(TimeSpan.FromSeconds(playerController.shootInterval))
            .Subscribe(_ => { StartCoroutine(ShootTimer()); });
    }

    void Update()
    {
        // �Q�[����Ԃ��v���C���łȂ��ꍇ�ɂ͏������s��Ȃ����������
        //if (arManager.ARStateReactiveProperty.Value != ARState.Play) {
        //    return;
        //}


#if UNITY_EDITOR
        // UI ���^�b�v���ꂽ�Ƃ��͏������Ȃ�(UI �̃{�^�����������炻����݂̂𔽉�������)
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
#else   // �X�}�z�p
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return;
        }
#endif

        // ���ˋ����Ȃ��ꍇ
        if (!playerController.IsShootPerimission) {
            // �������Ȃ�
            return;
        }


        // �����[�h����(�e�� 0 �Ń����[�h�@�\����̏ꍇ)
        if (playerController.BulletCount == 0 && playerController.isReloadModeOn && Input.GetMouseButtonDown(0)) {
            StartCoroutine(playerController.ReloadBullet());
        }

        // ���˔���(�e�����c���Ă���A�����[�h���s���łȂ��ꍇ)�@�������ςȂ��Ŕ��˂ł���
        //if (playerController.BulletCount > 0  && !playerController.isReloading && Input.GetMouseButton(0)) {

        //    // ���ˎ��Ԃ̌v��
        //    StartCoroutine(ShootTimer());
        //}
    }

    /// <summary>
    /// �p���I�Ȓe�̔��ˏ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootTimer() {
        //if (!isShooting) {
        //    isShooting = true;

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
    /// �e�̔���
    /// </summary>
    private void Shoot() {
        // �J�����̈ʒu���琳�ʂɌ������� Ray �𓊎�
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // �N���b�N�����ʒu�p
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

                // �ʏ�p
                //if (target.TryGetComponent(out eventBase)) {

                //    // ���ʂɂ��_���[�W�ʂ̏C�����v�Z 
                //    CalcDamage();

                //    // ���o
                //    PlayHitEffect(hit.point, hit.normal);
                //}

                // �_���[�W����(�f�o�b�O�p�B���ޗp)
                if (target.TryGetComponent(out enemy)) {
                    enemy.TriggerEvent(playerController.bulletPower);

                    // ���o
                    PlayHitEffect(hit.point, hit.normal);
                }
                
            //�@�����Ώۂ̏ꍇ
            } else if (target == hit.collider.gameObject) {
                //if (target.TryGetComponent(out parts)) {
                //    parts.CalcDamageParts(playerController.bulletPower);
                //} else 
                //if (target.TryGetComponent(out eventBase)) {

                // TODO �{�ԗp
                // �ʏ�B���ʂɂ��_���[�W�ʂ̏C�����v�Z 
                //CalcDamage();

                //eventBase.TriggerEvent(playerController.bulletPower);
                //}

                // ���o
                //PlayHitEffect(hit.point, hit.normal);


                // �f�o�b�O�p
                if (target.TryGetComponent(out enemy)) {
                    enemy.TriggerEvent(playerController.bulletPower);

                    // ���o
                    PlayHitEffect(hit.point, hit.normal);
                }
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

    /// <summary>
    /// ���ʂɂ��_���[�W�ʂ̏C�����v�Z
    /// </summary>
    /// <returns></returns>
    private void CalcDamage() {
        (int lastDamage, BodyRegionType hitRegionType) partsValue;

        if (target.TryGetComponent(out BodyRegionPartsController parts)) {
            partsValue = parts.CalcDamageParts(playerController.bulletPower);
        } else {
            partsValue = (playerController.bulletPower, BodyRegionType.Boby);
        }

        // ���ʂƃ_���[�W����
        eventBase.TriggerEvent(partsValue.lastDamage, partsValue.hitRegionType);
    }
}
