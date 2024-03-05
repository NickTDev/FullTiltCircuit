using UnityEngine;
using UnityEngine.Events;

public class CheckBalanceBoard : MonoBehaviour
{
    public UnityEvent balanceBoardReady;
    public UnityEvent balanceBoardNotReady;

    private bool hitOnce;

    public void CheckIfBalanceBoardReady()
    {
        if (WiiBoardTranslator.Inst == null)
        {
            Debug.LogError("WiiBoardTranslator is null! Make sure there is a Game Manager in the scene.");
            return;
        }

        if (hitOnce)
        {
            balanceBoardReady?.Invoke();
        }
        hitOnce = true;

        if (WiiBoardTranslator.Inst.IsConnected && WiiBoardTranslator.Inst.IsCalabrated)
        {
            balanceBoardReady?.Invoke();
        }
        else
        {
            balanceBoardNotReady?.Invoke();
        }
    }
}
