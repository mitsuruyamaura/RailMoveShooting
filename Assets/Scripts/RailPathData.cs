using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// レール移動用のパスデータ管理クラス
/// </summary>
public class RailPathData : MonoBehaviour
{
    [System.Serializable]
    public class PathDataDetail {

        [Tooltip("移動時間")]
        public float railMoveDuration;

        [Tooltip("移動地点とカメラの角度")]
        public Transform pathTran;

        [Tooltip("ミッションの発生有無。オンで発生")]
        public bool isMissionTrigger;
    }

    [Header("経路用のパスデータ群")]
    public PathDataDetail[] pathDataDetails;


    /// <summary>
    /// パスの移動時間の取得
    /// </summary>
    /// <returns></returns>
    public float[] GetRailMoveDurations() {
        return pathDataDetails.Select(x => x.railMoveDuration).ToArray();
    }

    /// <summary>
    /// パスの位置と回転情報の取得
    /// </summary>
    /// <returns></returns>
    public Transform[] GetPathTrans() {
        return pathDataDetails.Select(x => x.pathTran).ToArray();
    }

    /// <summary>
    /// ミッション発生有無の取得
    /// </summary>
    /// <returns></returns>
    public bool[] GetIsMissionTriggers(){
        return pathDataDetails.Select(x => x.isMissionTrigger).ToArray();
    }
}
