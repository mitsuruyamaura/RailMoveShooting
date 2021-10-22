using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathDataSO", menuName = "Create PathDataSO")]
public class PathDataSO : ScriptableObject {

    /// <summary>
    /// ƒ‹[ƒg‚Ìî•ñ
    /// </summary>
    [System.Serializable]
    public class RootData {
        public int rootNo;
        public List<PathData> pathDatasList = new List<PathData>();
    }

    public List<RootData> rootDatasList = new List<RootData>();
}
