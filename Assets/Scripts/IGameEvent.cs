using System.Collections;

/// <summary>
/// 
/// </summary>
public interface IGameEvent
{
    bool IsCompleted { get; }
    IEnumerator ExecuteEvent();
}
