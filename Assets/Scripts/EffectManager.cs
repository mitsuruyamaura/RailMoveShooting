using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    // Player�p
    [Header("���ˌ��p�̃G�t�F�N�g")]
    public GameObject muzzleFlashPrefab;

    [Header("�G�ɒe�����������Ƃ��̃G�t�F�N�g")]
    public GameObject hitEffectPrefab;

    // �G�p



    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }    
    }
}
