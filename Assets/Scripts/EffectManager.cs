using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    // Player用
    [Header("発射口用のエフェクト")]
    public GameObject muzzleFlashPrefab;

    [Header("敵に弾があたったときのエフェクト")]
    public GameObject hitEffectPrefab;

    // 敵用



    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }    
    }
}
