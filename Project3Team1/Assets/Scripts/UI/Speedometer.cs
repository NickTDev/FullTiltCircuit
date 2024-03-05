using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public float shakeAmplitude = .1f;
    public float shakeFreq = .1f;

    public Image speedBar;
    GameObject thePlayer;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.Find("Player");
        rb = thePlayer.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speedBar.fillAmount = rb.velocity.magnitude / 130;
        speedBar.fillAmount += rb.velocity.magnitude / 130 * shakeAmplitude * Mathf.PerlinNoise(0, rb.velocity.magnitude * Time.time * shakeFreq);
    }
}
