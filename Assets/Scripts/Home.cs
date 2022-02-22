using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    
    void Start()
    {
        TransitionManager.instance.FadeNextScene(1.0f, SceneName.MainGame);
    }
}
