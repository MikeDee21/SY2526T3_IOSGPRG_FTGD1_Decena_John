using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
   public void BTN_StartGame(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void BTN_ExitApplication()
    {
        Application.Quit(); 
    }
}
