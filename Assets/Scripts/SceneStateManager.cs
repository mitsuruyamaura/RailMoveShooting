using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour {

    public static SceneStateManager instance;
    private AsyncOperation async;


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

        yield return SceneManager.LoadSceneAsync(nextSceneName.ToString());
        TransitionManager.instance.FadeOut(2.0f);
    }
}