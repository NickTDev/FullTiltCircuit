using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseGame : MonoBehaviour
{
    public static event Action OnGameReset;

    public string resetSceneName = "StartScreen";
    public bool escapeResets;

    private void Awake()
    {
        if (!CompareTag("CloseGame"))
            Debug.LogError("Invalid tag! Must be \"CloseGame\"\n");

        DontDestroyOnLoad(gameObject);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("CloseGame");
        if (objs.Length > 1)
            Destroy(objs[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escapeResets && SceneManager.GetActiveScene().name != resetSceneName)
                ResetGame();
            else
                ExitGame();
        }
        if (!escapeResets && Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void ResetGame()
    {
        OnGameReset?.Invoke();
        SceneManager.LoadScene(resetSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting! ");
        Application.Quit();
    }
}
