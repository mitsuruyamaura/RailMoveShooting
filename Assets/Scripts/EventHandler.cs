using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameEventBase> gameEventList;

    /// <summary>
    /// 外部クラスより実行する
    /// </summary>
    public void StartHandlingEvents()
    {
        StartCoroutine(HandleEvents());
    }

    /// <summary>
    /// イベントを順番に処理する
    /// </summary>
    /// <returns></returns>
    private IEnumerator HandleEvents()
    {
        for(int i = 0; i < gameEventList.Count; i++)
        {
            // 現在のイベントが完了するまで次のイベントの開始を待つ
            yield return StartCoroutine(gameEventList[i].ExecuteEvent());
        }
    }
}
