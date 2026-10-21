using UnityEngine;
using UnityEngine.Events;

public class AnimationEventTool : MonoBehaviour
{
    public UnityEvent Use;

    public void TriggerEvent()
    {
        Use.Invoke();
    }
}
