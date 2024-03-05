using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiiBoardTranslator : MonoBehaviour
{
    public static WiiBoardTranslator Inst { get { return inst; } }
    private static WiiBoardTranslator inst;

    private Wiimote wiimote;

    public Vector2 JoystickValues { get { return isCalabrated ? joystickValues : Vector2.zero; } }
    private Vector2 joystickValues;

    public bool IsCalabrated { get { return isCalabrated; } }
    private bool isCalabrated = false;

    public bool IsConnected { get { return wiimote != null && wiimote.BalanceBoard != null; } }

    // Used for normilizing to [-1, 1] for joystickValues 
    public float maxHoriValue = 6000; // Highest I could get was ~6000 
    public float maxVertValue = 6000;

    private BalanceBoardData.BalanceBoardSensors RawSensors { get { return wiimote.BalanceBoard.RawSensorData; } }
    private BalanceBoardData.BalanceBoardSensors CalabratedSensors { get { return wiimote.BalanceBoard.RawSensorData - calabration; } }
    private BalanceBoardData.BalanceBoardSensors calabration;

    private void Awake()
    {
        if (inst != null)
            Debug.LogWarning("WiiBoardTranslator already exists! Ignoring previous one.");

        inst = this;
    }

    private void Start()
    {
        WiimoteManager.FindWiimotes();

        if (WiimoteManager.Wiimotes.Count <= 0)
        {
            Debug.LogWarning("No wiimotes found!");
            wiimote = null;
        }
        else
        {
            wiimote = WiimoteManager.Wiimotes[0];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            Calibrate();

        ReadWiimoteData();

        if (wiimote == null || wiimote.BalanceBoard == null)
            return;

        CalculateJoystickValues();
        //print(joystickValues);

        //print($"{CalabratedSensors.TopLeft}, {CalabratedSensors.TopRight}\n" +
        //      $"{CalabratedSensors.BottomLeft}, {CalabratedSensors.BottomRight}\n" +
        //      $"({horizontal}, {vertical})");
    }

    private void ReadWiimoteData()
    {
        if (wiimote == null)
            return;

        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();
        } while (ret > 0);
    }

    public void Calibrate()
    {
        Debug.Log("Calibrating...");
        calabration = RawSensors;

        isCalabrated = true;
    }

    private void CalculateJoystickValues()
    {
        int left = CalabratedSensors.BottomLeft + CalabratedSensors.TopLeft;
        int right = CalabratedSensors.BottomRight + CalabratedSensors.TopRight;
        float horizontal = right - left;
        //maxHoriValue = Mathf.Max(maxHoriValue, Mathf.Abs(horizontal));

        int forward = CalabratedSensors.TopLeft + CalabratedSensors.TopRight;
        int backward = CalabratedSensors.BottomLeft + CalabratedSensors.BottomRight;
        float vertical = forward - backward;
        //maxVertValue = Mathf.Max(maxVertValue, Mathf.Abs(vertical));

        joystickValues.x = horizontal / maxHoriValue;
        joystickValues.y = vertical / maxVertValue;
    }
}
