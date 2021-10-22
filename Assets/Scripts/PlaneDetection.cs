using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDetection : MonoBehaviour {
    private ARPlaneManager arPlaneManager;
    private bool isPlaneVisible = true;

    [SerializeField]
    private UIManager uiManager;

    private bool isActivePlane;


    private void Awake() {
        arPlaneManager = GetComponent<ARPlaneManager>();

        uiManager.InactiveARIntroductionText(false);

#if UNITY_ANDROID || UNITY_IOS
        uiManager.DisplayARIntroduction("カメラを操作して平面を感知しよう！");
#endif
    }

    void Update() {
        foreach (var plane in arPlaneManager.trackables) {
            plane.gameObject.SetActive(isPlaneVisible);
            if (plane.gameObject.activeSelf && !isActivePlane) {
                isActivePlane = true;
#if UNITY_ANDROID || UNITY_IOS
                //uiManager.DisplayARIntroduction("感知した場所をタップしよう！");
#endif
            }
        }
    }

    /// <summary>
    /// ARPlane の表示オンオフ切り替え
    /// </summary>
    public void SetAllPlaneActivate(bool isSwitch) {
        isPlaneVisible = isSwitch;

#if UNITY_ANDROID || UNITY_IOS
        if (isSwitch == false) {
            //uiManager.InactiveARIntroductionText(isSwitch);
#endif
    }
}
