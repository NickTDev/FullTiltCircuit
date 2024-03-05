using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LapManager : MonoBehaviour
{
    private int currentLap;
    private int maxLaps;

    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Text lapText;

    // Start is called before the first frame update
    void Start()
    {
        trackCheckpoints.OnPlayerFirstCheckpoint += TrackCheckpoints_OnPlayerFirstCheckpoint;

        currentLap = 0;
        maxLaps = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLap > maxLaps)
        {
            GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>().StopTimer();
            SceneManager.LoadScene("EndScene");
        }
    }

    private void TrackCheckpoints_OnPlayerFirstCheckpoint(object sender, System.EventArgs e)
    {
        currentLap++;
        lapText.text = "Lap: " + currentLap.ToString() + "/3";
    }
}
