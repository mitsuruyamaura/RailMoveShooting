using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���򂲂Ƃ̃p�X�f�[�^�̃f�[�^�x�[�X
/// </summary>
[CreateAssetMenu(fileName = "BranchDataSO", menuName = "Create BranchDataSO")]
public class BranchDataSO : ScriptableObject
{
    [System.Serializable]
    public class BranchData {
        public int branchNo;
        public RailPathData[] railPathDatas;

        // ���ɂ�����Βǉ�����

    }

    [Header("���򂲂Ƃ̃p�X�f�[�^�Q")]
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

    //[Header("�X�e�[�W���Ƃ̃p�X�f�[�^�Q")]
    //public List<StagePathData> stagePathDatasList = new List<StagePathData>();
}
