using UnityEngine;

[System.Serializable]
public class PathData {

    [Tooltip("移動時間")]
    public float scrollDuration;

    [Tooltip("移動地点")]
    public Transform pathTran;

    [Tooltip("カメラの角度")]
    public Vector3 cameraRote;
}
