using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分岐ごとのパスデータのデータベース
/// </summary>
[CreateAssetMenu(fileName = "BranchDataSO", menuName = "Create BranchDataSO")]
public class BranchDataSO : ScriptableObject
{
    [System.Serializable]
    public class BranchData {
        public int branchNo;
        public RailPathData[] railPathDatas;

        // 他にもあれば追加する

    }

    [Header("分岐ごとのパスデータ群")]
    public List<BranchData> branchDatasList = new List<BranchData>();

    //[System.Serializable]
    //public class StagePathData {
    //    public int stageNo;
    //    public List<RootData> rootDatas;

    //    [System.Serializable]
    //    public class RootData {
    //        public int rootNo;
    //        public List<BranchData> branchDatas;

    //        [System.Serializable]
    //        public class BranchData {
    //            public int branchNo;
    //            public BranchDirectionType branchDirectionType;
    //            public RailPathData[] railPathDatas;
    //        }
    //    }
    //}

    //[Header("ステージごとのパスデータ群")]
    //public List<StagePathData> stagePathDatasList = new List<StagePathData>();
}
