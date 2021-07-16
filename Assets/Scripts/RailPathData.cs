using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レール移動用のパスデータ管理クラス
/// </summary>
public class RailPathData : MonoBehaviour
{
    [SerializeField, Tooltip("移動時間")]
    private float[] railMoveDurations;

    [SerializeField, Tooltip("移動地点とカメラの角度")]
    private Transform[] pathTrans;

    /// <summary>
    /// パスの移動時間の取得
    /// </summary>
    /// <returns></returns>
    public float[] GetRailMoveDurations() {
        return railMoveDurations;
    }

    /// <summary>
    /// パスの位置と回転情報の取得
    /// </summary>
    /// <returns></returns>
    public Transform[] GetPathTrans() {
        return pathTrans;
    }
}
