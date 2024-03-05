using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MotorEngine : MonoBehaviour
{
    Rigidbody rb;
    AudioSource engine;

    Vector3 currentVel;
    Vector3 previousVel;

    [HideInInspector] public bool reverse;

    [HideInInspector] public bool shiftReady;
    [HideInInspector] public bool shift;

    [SerializeField] float topSpeed = 30;
    //[SerializeField] float topSpeedPitch = 3;
    [SerializeField] float minPitch = 0.5f;
    //[SerializeField] float swell = 1.2f;
    [SerializeField] float shiftTrigger = 15;
    [SerializeField] float shiftNum = 1.2f;

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        engine = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedTacker();
        Pitch();

        //Debug.Log(currentVel.magnitude);
    }

    private void FixedUpdate()
    {
        previousVel = rb.velocity;
    }

    void SpeedTacker()
    {
        currentVel = rb.velocity;

        if (previousVel.magnitude > currentVel.magnitude)
        {
            reverse = true;
        }
        else
            reverse = false;

    }

    void Pitch()
    {
        
        if (currentVel.magnitude <= 1 || engine.pitch < minPitch)
        {
            engine.pitch = minPitch;
        }
        else
        {
            engine.pitch = (currentVel.magnitude / 30);
        }
        /*
        if (currentVel.magnitude >= topSpeed && reverse == false)
        {
            engine.pitch = topSpeedPitch;
        }
        else if (currentVel.magnitude >= previousVel.magnitude && reverse == false && shift == false)
        {
            engine.pitch += swell * Time.deltaTime;
        }
        else if (reverse == true)
        {
            engine.pitch -= swell *2 * Time.deltaTime;
        }
        */
        
        
        
    }

    void GearShifting()
    {
        if (rb.velocity.magnitude >= shiftTrigger && currentVel.magnitude < topSpeed && reverse == false && shiftReady == true)
        {
            shift = true;
            shiftReady = false;
        }

        if (shift == true)
        {
            engine.pitch = shiftNum;
            shift = false;
        }
        if (rb.velocity.magnitude < shiftTrigger)
        {
            shiftReady = true;
        }
    }


}
