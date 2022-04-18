using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分岐ごとのパスデータのデータベース
/// </summary>
[CreateAssetMenu(fileName = "StagePathDataSO", menuName = "Create StagePathDataSO")]
public class StagePathDataSO : ScriptableObject
{
    [System.Serializable]
    public class StagePathData {

        [System.Serializable]
        public class BranchData {
            public BranchDirectionType branchDirectionType;   // 選択肢用の矢印の方向
            public RailPathData railPathData;

            // 他にもあれば追加する

        }

        [Header("分岐ごとのパスデータ群")]
        public List<BranchData> branchDatasList;
    }

    [Header("ステージごとのパスデータ群")]
    public List<StagePathData> stagePathDatasList = new List<StagePathData>();
}
