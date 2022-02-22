using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour {
    public static TransitionManager instance;

    [SerializeField]
    private Fade fade;

    [SerializeField]
    private FadeImage fadeImage;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// フェードイン ＆ フェードアウト
    /// </summary>
    /// <param name="duration"></param>
    public void FadeInAndFadeOut(float duration) {

        fade.FadeIn(duration, () => fade.FadeOut(2.0f));
        Debug.Log("Fade_Opening");
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="duration"></param>
    public void FadeOut(float duration) {

        fade.FadeOut(2.0f);
    }


    public void FadeNextScene(float duration, SceneName nextSceneName) {
        //fade.FadeIn(duration, () => fade.FadeOut(2.0f));

        fade.FadeIn(duration);

        SceneStateManager.instance.PrepareChangeScene(nextSceneName);
        Debug.Log("Fade_Restart");
    }
}