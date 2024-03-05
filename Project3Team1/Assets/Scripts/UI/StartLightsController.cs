using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartLightsController : MonoBehaviour
{
    public Image[] lights;
    public RaceCountdown countdown;
    public float disappearTime = 0.5f;

    public Color goColor = new Color(0x75 / 0xFF, 0xFF / 0xFF, 0x33 / 0xFF);

    void Awake()
    {
        Debug.Assert(lights != null && lights.Length > 0, "Lights are not set!");
    }

    void Update()
    {
        int index = (int)(countdown.PercentTimeToGo * lights.Length);
        index = Mathf.Clamp(index, 0, lights.Length - 1);
        lights[index].enabled = true;
    }

    public void SetLightsToGo()
    {
        foreach (Image i in lights)
        {
            i.color = goColor;
        }
    }

    public void StartDisappear()
    {
        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearTime);

        gameObject.SetActive(false);
    }
}
