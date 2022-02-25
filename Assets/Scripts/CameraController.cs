using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public void ClearCameraRoll(Vector3 pos, Vector3 rotation, float[] durations) {

        Sequence sequence = DOTween.Sequence();

        sequence.Append(mainCamera.transform.DOMove(pos, durations[0]).SetEase(Ease.InQuart));
        sequence.AppendInterval(0.5f);
        sequence.Append(mainCamera.transform.DORotate(rotation, durations[1])
            .SetEase(Ease.InQuart))
            .OnComplete(() => {
                SoundManager.instance.MuteBGM();
                SoundManager.instance.PlaySE(SoundManager.SE_Type.GameClear);
            });
    }
}