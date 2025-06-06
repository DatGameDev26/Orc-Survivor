using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] RedWall redWall;

    Animator animator;
    bool isPause = false;
    bool beginGameOver = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkPause();
    }

    private void checkPause()
    {
        if (beginGameOver) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            isPause = !isPause;
            pausePanel.SetActive(isPause);

            if (isPause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    
    public void EndGame(string result)
    {
        if (beginGameOver) return;
        beginGameOver = true;
        animator.SetTrigger(result);
        if(result == "Win")
        {
            redWall.Terminate();
        }
    }

    public bool isGameEnd() { return beginGameOver; }
}
