using UnityEngine;

public class TimerTest : MonoBehaviour
{
    public float stateTimer;
    public float stateDuration = 3;
    void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    [ContextMenu("Set Timer")]
    public void SetTimer()
    {
        stateTimer = stateDuration;
    }
}
