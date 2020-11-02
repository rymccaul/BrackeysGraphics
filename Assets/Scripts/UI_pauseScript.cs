using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_pauseScript : MonoBehaviour
{
    private bool isPaused = false;

    void Update(){
      if(Input.GetKeyDown(KeyCode.Escape))
        clickPauseButton();
    }

    // Start is called before the first frame update
    public void clickPauseButton()
    {
        if (isPaused){
            Debug.Log("Unpaused.");
            resume();
        }
        else
        {
            Debug.Log("Paused.");
            pause();
        }

        return;
    }

    private void resume(){
      isPaused = false;
      return;
    }

    private void pause(){
      isPaused = true;
      return;
    }
}
