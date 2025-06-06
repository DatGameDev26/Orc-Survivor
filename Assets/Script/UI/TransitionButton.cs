using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionButton : MonoBehaviour
{
    [SerializeField] int sceneTransitionIndex;

    public void transitionToChosenScene()
    {
        SceneManager.LoadScene(sceneTransitionIndex);
        Time.timeScale = 1.0f;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void reloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
