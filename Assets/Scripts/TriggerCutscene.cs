using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("MainCamera"))
        {
            //playableDirector.Play();
            
            Debug.Log("カットシーン 開始");

            // カットシーン再生
            StartCoroutine(PlayCutscene());
        }

        if (other.TryGetComponent(out PlayerController player)) {

            Debug.Log("カットシーン 開始");

            // TODO プレイヤーの移動と行動制御

            // カットシーン再生
            StartCoroutine(PlayCutscene());
        }
    }

    /// <summary>
    /// カットシーン再生
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayCutscene() {
        
        playableDirector.Play();

        // カットシーンが終了するまで待機①
        // while (playableDirector.state == PlayState.Playing) {
        //     yield return null;
        // }

        // カットシーンが終了するまで待機②
        yield return new WaitUntil(() => playableDirector.state != PlayState.Playing);
        
        Debug.Log("カットシーン 終了");

        // TODO カットシーン後の処理
        
    }
}