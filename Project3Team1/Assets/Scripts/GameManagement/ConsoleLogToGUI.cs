using System.IO;
using System;
using UnityEngine;

public class ConsoleLogToGUI : MonoBehaviour
{
    const int LOG_LEN = 700; // Number of characters 

    string theLog = "";
    bool showLog = false;
    string filename = "";

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            showLog = !showLog;

            if (showLog)
                Debug.Log("Log Shown");
            else
                Debug.Log("Log Hidden");
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        // Show on screen 
        theLog = theLog + "\n" + logString;
        if (theLog.Length > LOG_LEN) { theLog = theLog.Substring(theLog.Length - LOG_LEN); }

        // Don't write to a file if we are in the editor 
        if (Application.isEditor)
            return;

        // Stash in a file 
        // Set file name if not done yet 
        if (filename == "")
        {
            // Create the directory for the logs if it didn't exist 
            string dir = Application.dataPath + "/Logs/";
            Directory.CreateDirectory(dir);

            // Get the date and time for the file name 
            string time = DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss");
            filename = dir + "Log-" + time + ".txt";
            File.WriteAllText(filename, "START OF LOG " + DateTime.Now.ToString("yyyy'-'MM'-'dd' 'ddd' 'HH':'mm':'ss") + '\n');
        }

        try
        {
            // Write to the log 
            File.AppendAllText(filename, logString + "\n");
        }
        catch (Exception e)
        {
            theLog = theLog + "\n" + "Error writing to file! " + e.Message;
        }
    }

    private void OnGUI()
    {
        if (showLog)
            GUI.TextArea(new Rect(10, 10, Screen.width - 20, 370), theLog);
    }
}
