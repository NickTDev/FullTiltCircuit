using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void changeToScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
        // Bypass key 
        if (Input.GetKeyDown(KeyCode.P))
            changeToScene();
    }
}
