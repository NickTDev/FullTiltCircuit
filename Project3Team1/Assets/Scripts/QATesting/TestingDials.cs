using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerVars
{
    // A ton of properties to make it WAY easier to reference each of these later 
    public float Speed { get { return GetPlayerMove.MaxSpeed; } set { GetPlayerMove.MaxSpeed = value; } }
    public float Drag { get { return GetPlayerRB.drag; } set { GetPlayerRB.drag = value; } }
    public float RotForce { get { return GetPlayerMove.rotationForce; } set { GetPlayerMove.rotationForce = value; } }
    public float Sideways { get { return GetPlayerSideways.maxSidewaysForce; } set { GetPlayerSideways.maxSidewaysForce = value; } }
    public float Brake { get { return GetPlayerMove.brakeForce; } set { GetPlayerMove.brakeForce = value; } }

    // Lazy init to get player classes 
    private PlayerMovement GetPlayerMove
    {
        get
        {
            if (playerMove == null)
                playerMove = playerObject.GetComponent<PlayerMovement>();
            return playerMove;
        }
    }
    private PlayerSidewaysFriction GetPlayerSideways
    {
        get
        {
            if (playerSideways == null)
                playerSideways = playerObject.GetComponent<PlayerSidewaysFriction>();
            return playerSideways;
        }
    }
    private Rigidbody GetPlayerRB
    {
        get
        {
            if (playerRB == null)
                playerRB = playerObject.GetComponent<Rigidbody>();
            return playerRB;
        }
    }

    [SerializeField] private GameObject playerObject;
    private PlayerMovement playerMove;
    private PlayerSidewaysFriction playerSideways;
    private Rigidbody playerRB;
}

// Struct just to make it collapse in the inspector 
[System.Serializable]
public struct Sliders
{
    public Slider speed;
    public Slider drag;
    public Slider rotForce;
    public Slider sideways;
    public Slider brake;
}
[System.Serializable]
public struct ValueTexts
{
    public TMP_Text speed;
    public TMP_Text drag;
    public TMP_Text rotForce;
    public TMP_Text sideways;
    public TMP_Text brake;
}

public class TestingDials : MonoBehaviour
{
    public PlayerVars playerVars;
    public Sliders sliders;
    public ValueTexts texts;
    public Canvas dialCanvas;

    [Header("Ranges (x = min, y = max)")]
    public Vector2 speedRange;
    public Vector2 dragRange;
    public Vector2 rotForceRange;
    public Vector2 sidewaysRange;
    public Vector2 brakeRange;

    private void Awake()
    {
        // Man I did this in a dumb way but hey 
        texts.speed.text = playerVars.Speed.ToString("0.00");
        sliders.speed.minValue = speedRange.x;
        sliders.speed.maxValue = speedRange.y;
        sliders.speed.value = playerVars.Speed;
        sliders.speed.onValueChanged.AddListener(delegate { OnSliderChangedSpeed(); });

        texts.drag.text = playerVars.Drag.ToString("0.00");
        sliders.drag.minValue = dragRange.x;
        sliders.drag.maxValue = dragRange.y;
        sliders.drag.value = playerVars.Drag;
        sliders.drag.onValueChanged.AddListener(delegate { OnSliderChangedDrag(); });

        texts.rotForce.text = playerVars.RotForce.ToString("0.00");
        sliders.rotForce.minValue = rotForceRange.x;
        sliders.rotForce.maxValue = rotForceRange.y;
        sliders.rotForce.value = playerVars.RotForce;
        sliders.rotForce.onValueChanged.AddListener(delegate { OnSliderChangedRotForce(); });

        texts.sideways.text = playerVars.Sideways.ToString("0.00");
        sliders.sideways.minValue = sidewaysRange.x;
        sliders.sideways.maxValue = sidewaysRange.y;
        sliders.sideways.value = playerVars.Sideways;
        sliders.sideways.onValueChanged.AddListener(delegate { OnSliderChangedSideways(); });

        texts.brake.text = playerVars.Brake.ToString("0.00");
        sliders.brake.minValue = brakeRange.x;
        sliders.brake.maxValue = brakeRange.y;
        sliders.brake.value = playerVars.Brake;
        sliders.brake.onValueChanged.AddListener(delegate { OnSliderChangedBrake(); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ToggleMenu();

        if (Input.GetKeyDown(KeyCode.P))
            PrintToFile();
    }

    private void ToggleMenu()
    {
        dialCanvas.enabled = !dialCanvas.enabled;
    }

    private void PrintToFile()
    {
        if (Application.isEditor)
        {
            Debug.Log("Print to file now! (Will not print to file in editor)");
            return;
        }

        // Create the directory for the logs if it didn't exist 
        string dir = Application.dataPath + "/Sprint2_Testing/";
        Directory.CreateDirectory(dir);

        // Get the date and time 
        string time = DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss");
        string filename = dir + "TestVars.csv";

        string contents = $"{time}, {playerVars.Speed}, {playerVars.Drag}, {playerVars.RotForce}, {playerVars.Sideways}, {playerVars.Brake}\n";

        if (!File.Exists(filename))
            File.AppendAllText(filename, "TimeDate, Speed, Drag, RotForce, Sideways, Brake\n");

        File.AppendAllText(filename, contents);

        Debug.Log($"Variables Saved at {filename}");
    }

    private void OnSliderChangedSpeed()
    {
        playerVars.Speed = sliders.speed.value;
        texts.speed.text = sliders.speed.value.ToString("0.00");
    }

    private void OnSliderChangedDrag()
    {
        playerVars.Drag = sliders.drag.value;
        texts.drag.text = sliders.drag.value.ToString("0.00");
    }

    private void OnSliderChangedRotForce()
    {
        playerVars.RotForce = sliders.rotForce.value;
        texts.rotForce.text = sliders.rotForce.value.ToString("0.00");
    }

    private void OnSliderChangedSideways()
    {
        playerVars.Sideways = sliders.sideways.value;
        texts.sideways.text = sliders.sideways.value.ToString("0.00");
    }

    private void OnSliderChangedBrake()
    {
        playerVars.Brake = sliders.brake.value;
        texts.brake.text = sliders.brake.value.ToString("0.00");
    }

}
