using System.Diagnostics;
using UnityEngine;

public class OpenWiiBalanceWalker : MonoBehaviour
{
    Process wiiBalanceWalker;

    private void Start()
    {
        wiiBalanceWalker = new Process();
        wiiBalanceWalker.StartInfo.UseShellExecute = false;

        if (Application.isEditor)
        {
            wiiBalanceWalker.StartInfo.FileName = Application.dataPath + "/../../WiiBalanceWalker_v0.4/Server/WiiBalanceWalker_v0.4/WiiBalanceWalker.exe";
        }
        else
        {
            wiiBalanceWalker.StartInfo.FileName = Application.dataPath + "/Server/WiiBalanceWalker_v0.4/WiiBalanceWalker.exe";
        }

        wiiBalanceWalker.Start();
    }

    private void OnDestroy()
    {
        wiiBalanceWalker?.Close();
    }
}
