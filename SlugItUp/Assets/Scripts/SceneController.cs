using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void sceneEvent(int buildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex: buildIndex);
    }

    public void exit()
    {
        Application.Quit();
    }
}
