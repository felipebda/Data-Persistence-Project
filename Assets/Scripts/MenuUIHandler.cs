using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public InputField playerNameInputField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMainScene()
    {
        //Save the name of the player 
        SavePlayerName();
        //Move to the main Scene
        LoadToMainScene();
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void SavePlayerName()
    {
        PlayerInfoManager.Instance.playerName = playerNameInputField.text;
    }

    private void LoadToMainScene()
    {
        SceneManager.LoadScene(1);
    }
}
