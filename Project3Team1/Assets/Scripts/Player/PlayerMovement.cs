using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public UnityEvent onFirstInput;

    private Rigidbody rb;

    public float rotationForce;
    private Vector3 rotationPt;

    // Set up as a property so that the acceleration force is calculated when set via script 
    public float MaxSpeed 
    {
        get { return maxSpeed; }
        set
        {
            maxSpeed = value;
            accelerateForce = maxSpeed * rb.mass * rb.drag /** 0.1f*/;
        }
    }

    public GameObject bikeSprite;
    public float maxTurnAngle;
    [SerializeField] private float maxSpeed;
    private float accelerateForce;
    public float brakeForce;
    public float boostForce;
    private float boost = 1f;
    public LayerMask trailLayer;
    //public float maxBackUpSpeed;

    [HideInInspector] public float currentForce = 0;
    private float currentRotationForce = 0;

    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    private bool started = false;

    private Vector2 input;
    public bool useInput = true;
    public void SetUseInput(bool useInput) { this.useInput = useInput; }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rotationPt = transform.position;

        accelerateForce = maxSpeed * rb.mass * rb.drag /** 0.1f*/;
    }

    // Update is called once per frame
    void Update()
    {
        BoostOnTrail();

        ReadInput();
        InterpretInput();
        //UpdateSprite();
    }

    private void FixedUpdate()
    {
        ApplyForce();
    }

    private void BoostOnTrail()
    {
        if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out RaycastHit hit, 10, trailLayer))
            boost = boostForce;
        else
            boost = 1f;
    }

    private void ReadInput()
    {
        if (!useInput)
        {
            input = Vector2.zero;
            return;
        }

        if (WiiBoardTranslator.Inst != null && WiiBoardTranslator.Inst.IsConnected && WiiBoardTranslator.Inst.IsCalabrated)
        {
            input = WiiBoardTranslator.Inst.JoystickValues;
        }
        else
        {
            Debug.LogWarning("Balance board not connected or not calabrated! Defaulting to unity input. ");

            input.x = Input.GetAxis(HORIZONTAL_AXIS);
            input.y = Input.GetAxis(VERTICAL_AXIS);
        }
    }

    private void InterpretInput()
    {
        // Turning 
        currentRotationForce = rotationForce * input.x;
        currentForce = Mathf.Clamp(currentRotationForce, -rotationForce, rotationForce);

        // Driving 
        currentForce = accelerateForce * input.y;
        currentForce = Mathf.Clamp(currentForce, -brakeForce, accelerateForce);

        // Start Timer on input 
        if (!started && input != Vector2.zero)
        {
            started = true;
            onFirstInput?.Invoke();
        }
    }

    private void UpdateSprite()
    {
        bikeSprite.transform.rotation = Quaternion.AngleAxis(input.x * maxTurnAngle, Vector3.forward);
    }

    private void ApplyForce()
    {
        rb.AddForce(transform.forward * currentForce * boost);

        // I don't think we need to clamp backwards speed actually 
        //if (currentForce == -brakeForce)
        //{
        //    rb.AddForce(transform.forward * currentForce);
        //    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxBackUpSpeed);
        //}
        //else
        //{
        //    rb.AddForce(transform.forward * currentForce);
        //}

        rb.AddTorque(rotationPt * currentRotationForce);
    }
}
