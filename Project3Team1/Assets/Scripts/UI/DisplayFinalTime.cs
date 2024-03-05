using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFinalTime : MonoBehaviour
{
    public Text time;

    void Start()
    {
        time.text = "Time: " + TimeSpan.FromSeconds(Timer.elapsedTime).ToString("mm\\:ss\\.ff");
        Timer.elapsedTime = 0;
    }
}
