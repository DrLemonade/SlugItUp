using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public AudioClip selectSound;

    public void sceneEvent(int buildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex: buildIndex);
    }

    public void exit()
    {
        Application.Quit();
    }

    public void playSelectSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(selectSound);
    }

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
