using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera[] virtualCameras;

    /// <summary>
    /// ƒJƒƒ‰Ø‚è‘Ö‚¦
    /// </summary>
    /// <param name="index"></param>
    public void SwitchCamera(int index) {

        for (int i = 0; i < virtualCameras.Length; i++) {
            virtualCameras[i].Priority = (i == index) ? 10 : 0;
        }
    }
}
