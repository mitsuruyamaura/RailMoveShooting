using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class GameEventCutscene : GameEventBase
{
    [SerializeField] private PlayableDirector playableDirector;
    
    public override IEnumerator ExecuteEvent() {
        playableDirector.Play();

        // カットシーンが終了するまで待機
        yield return new WaitUntil(() => playableDirector.state != PlayState.Playing);

        Debug.Log("カットシーン 終了");

        // TODO カットシーン後の処理
    }
}
