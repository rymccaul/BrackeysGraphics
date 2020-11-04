using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_pauseScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;

    void Update(){
      if(Input.GetKeyDown(KeyCode.Escape))
        clickPauseButton();
    }

    // Start is called before the first frame update
    public void clickPauseButton()
    {
        if (isPaused){
            resume();
        }
        else
        {
            pause();
        }

        return;
    }

    private void resume(){
      isPaused = false;
      pauseMenu.SetActive(false);
      Time.timeScale = 1f;
      return;
    }

    private void pause(){
      isPaused = true;
      pauseMenu.SetActive(true);
      Time.timeScale = 0f;
      return;
    }
}
