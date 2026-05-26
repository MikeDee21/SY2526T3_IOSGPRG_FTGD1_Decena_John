using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
   public void BTN_StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void BTN_ExitApplication()
    {
        Application.Quit(); 
    }
}
