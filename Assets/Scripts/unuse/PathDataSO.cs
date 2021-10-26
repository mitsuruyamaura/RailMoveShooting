using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 未使用
/// </summary>
[CreateAssetMenu(fileName = "PathDataSO", menuName = "Create PathDataSO")]
public class PathDataSO : ScriptableObject {

    /// <summary>
    /// ルートの情報
    /// </summary>
    [System.Serializable]
    public class RootData {
        public int rootNo;
        public List<PathData> pathDatasList = new List<PathData>();
    }

    public List<RootData> rootDatasList = new List<RootData>();
}
