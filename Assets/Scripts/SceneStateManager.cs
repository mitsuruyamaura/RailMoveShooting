using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour {
    public static SceneStateManager instance;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }


    public void PrepareChangeScene(SceneName nextSceneName) {
        StartCoroutine(ChangeScene(nextSceneName));
    }


    private IEnumerator ChangeScene(SceneName nextSceneName) {

        yield return new WaitForSeconds(1.0f);

        var async = SceneManager.LoadSceneAsync(nextSceneName.ToString());
        async.allowSceneActivation = false;

        yield return new WaitUntil(() => async.progress >= 0.9f);

        async.allowSceneActivation = true;

        //TransitionManager.instance.FadeOut(2.0f);

        TransitionManager.instance.FadeInAndFadeOut(0);
    }
}