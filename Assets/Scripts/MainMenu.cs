using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject NewGameButton;
    public GameObject LoadGameButton;
    public GameObject ExitGameButton;
    public GameObject LoadMenu;

    private static string SaveSelected;

    public void NewGame()
    {
        Debug.Log(Application.persistentDataPath);

    }

    public void LoadScreenChange()
    {
        NewGameButton.SetActive(false);
        LoadGameButton.SetActive(false);
        ExitGameButton.SetActive(false);
        LoadMenu.SetActive(true);
    }

    public void LoadGame(string LoadSelected)
    {
        print($"Save Loaded: {LoadSelected}");
        SaveSelected = LoadSelected;
        SaveTheBooks.LoadGame(LoadSelected);
        print(Application.persistentDataPath);
    }

    public static string SelectedSaveLoad()
    {
        return SaveSelected;
    }

    public void Back()
    {
        NewGameButton.SetActive(true);
        LoadGameButton.SetActive(true);
        ExitGameButton.SetActive(true);
        LoadMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
