using UnityEngine;

public class PlayerSidewaysFriction : MonoBehaviour
{
    public float maxSidewaysForce = 0.1f;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Taking the dot product between 2 normilized vectors 
        // This returns 1 when the angle between them is nearly 0 and -1 for an angle of 180 
        float forceMagnitude = Mathf.Abs(Vector3.Dot(rb.velocity.normalized, transform.right));
        forceMagnitude *= maxSidewaysForce; // Make sure to amplify by sideways force 

        // Slow down the player when moving sideways 
        rb.AddForce(-rb.velocity * forceMagnitude);
    }
}
