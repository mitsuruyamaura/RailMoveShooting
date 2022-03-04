using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = "Create StageSO")]
public class StageSO : ScriptableObject
{
    public List<StagePathDataSO> stageDataList = new List<StagePathDataSO>();
}
