using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RootPathDataSO", menuName = "Create RootPathDataSO")]
public class RootPathDataSO : ScriptableObject
{
    [System.Serializable]
    public class RootPathData {
        public int rootNo;
        public List<RailPathData> railPathDatasList = new List<RailPathData>();
    }

    public List<RootPathData> rootPathDatasList = new List<RootPathData>();
}
