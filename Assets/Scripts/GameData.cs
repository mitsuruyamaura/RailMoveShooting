using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public ReactiveProperty<int> scoreReactiveProperty;

    public bool useCinemachine;

    [Header("E‚Á‚½•Ší‚Ì“o˜^—pƒŠƒXƒg")]
    public List<WeaponData> weaponDatasList = new List<WeaponData>();


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void InitializeGameData() {
        scoreReactiveProperty = new ReactiveProperty<int>();
    }

    /// <summary>
    /// •Šíƒf[ƒ^‚Ì“o˜^
    /// </summary>
    /// <param name="weaponData"></param>
    public void AddWeaponData(WeaponData weaponData) {
        weaponDatasList.Add(weaponData);
    }
}
