using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRotate : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float turnSpeed;
    private Quaternion angle;

    private void Awake()
    {
        gameObject.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        angle = Quaternion.Slerp(angle, playerTransform.rotation, Time.deltaTime * turnSpeed);
        transform.rotation = angle;
    }
}
