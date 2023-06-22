using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventBase : MonoBehaviour, IGameEvent
{
    public bool IsCompleted { get; }
    
    public virtual IEnumerator ExecuteEvent() {
        yield return null;
    }
}
