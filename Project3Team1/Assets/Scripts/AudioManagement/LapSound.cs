using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LapSound : MonoBehaviour
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;

    [SerializeField] AudioSource music;

    public AudioClip lapSound;
    public AudioClip finalLapSound;
    private AudioSource source;

    private int currentLap;
    public int numLaps = 3;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        Debug.Assert(trackCheckpoints != null, "trackCheckpoints not set!");
        Debug.Assert(lapSound != null, "lapSound not set!");
        Debug.Assert(finalLapSound != null, "finalLapSound not set!");
        Debug.Assert(numLaps > 0, "numLaps is 0!");
    }

    private void OnEnable()
    {
        trackCheckpoints.OnPlayerFirstCheckpoint += OnLapStart;
    }

    private void OnDisable()
    {
        trackCheckpoints.OnPlayerFirstCheckpoint -= OnLapStart;
    }

    private void OnLapStart(object sender, System.EventArgs e)
    {
        currentLap++;

        if (currentLap >= numLaps)
        {
            source.PlayOneShot(finalLapSound);
            StartCoroutine(volume());
        }
        else
        {
            source.PlayOneShot(lapSound);
            StartCoroutine(volume());
        }
        if (currentLap > numLaps)
            Debug.LogError("Current lap is greater than num laps! Make sure numLaps is the same as the Lap Manager.");
    }

    IEnumerator volume()
    {
        music.volume = music.volume * 0.5f;
        yield return new WaitWhile(() => source.isPlaying);
        music.volume = music.volume * 2;
    }
}
