using UnityEngine;
using UnityEngine.Events;

public class RaceCountdown : MonoBehaviour
{
    public UnityEvent onGo;

    public float PercentTimeToGo { get { return 1 - Mathf.Clamp01((goTime - Time.time) / timeToGo); } }

    [SerializeField] private float timeToGo = 6.3f;

    private float goTime;
    private bool done = false;

    private void Start()
    {
        goTime = Time.time + timeToGo;
    }

    void Update()
    {
        if (!done && goTime <= Time.time)
        {
            onGo?.Invoke();
            done = true;
        }
    }
}
