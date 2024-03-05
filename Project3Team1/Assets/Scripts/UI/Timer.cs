using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text time;
    public static float elapsedTime = 0;
    private bool run = false;

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            elapsedTime += Time.deltaTime;
            //elapsedTime = Mathf.Round(elapsedTime * 100.0f) / 100.0f;
            time.text = TimeSpan.FromSeconds(elapsedTime).ToString("mm\\:ss\\.ff");
        }
    }

    public void StartTimer()
    {
        run = true;
    }

    public void StopTimer()
    {
        run = false;
    }
}
