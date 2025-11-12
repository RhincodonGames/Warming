using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemPanel : MonoBehaviour
{
    public void Save()
    {
        //saves game to new load file
        //max load files will be 3 (to reduce game strain)
    }

    //Load Selected Save File
    public void Load()
    {
        //Opens Save Files
        //Player Selects File
        //Loads back to that save
    }

    public void ReturnToMainMenu()
    {
        //if (!isSaved)
        //{
        //    //Display warning dialogue and give player chance to save or ignore warning
        //}

        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");

    }

    //Method for adjusting audio
    //
    //
    //
}