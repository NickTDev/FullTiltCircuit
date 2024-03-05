using System;
using UnityEngine;
using PathCreation;

public class PlayerOffTrackSlowdown : MonoBehaviour
{
    public static event Action onTrackEnter;
    public static event Action onTrackExit;

    public PathCreator track;
    public float trackWidth = 20;
    public float offDrag = 3;
    private float onDrag;

    bool onTrack = true;

    Rigidbody rb;

    Vector3 debugPoint;

    private void Awake()
    {
        if (track == null)
            Debug.LogError("Track not set! Be sure to set it in the editor");

        rb = GetComponent<Rigidbody>();
        onDrag = rb.drag;
    }

    private void Update()
    {
        CheckIfOnTrack();

        if (onTrack)
        {
            rb.drag = onDrag;
        }
        else
        {
            rb.drag = offDrag;
        }
    }

    private void CheckIfOnTrack()
    {
        if (track == null)
            return;

        // Of course this function just exists 
        Vector3 closestPoint = track.path.GetClosestPointOnPath(transform.position);

        // Compare only the x and z and ignore the y 
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        closestPoint.y = 0;

        debugPoint = closestPoint;
        //print(Vector3.Distance(closestPoint, transform.position));

        // if (On the Track) 
        if (Vector3.Distance(closestPoint, transform.position) < trackWidth) // This could be faster but it works 
        {
            if (!onTrack)
            {
                onTrack = true;
                onTrackEnter?.Invoke();
            }
        }
        else
        {
            if (onTrack)
            {
                onTrack = false;
                onTrackExit?.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(debugPoint, 0.1f);
    }
}
