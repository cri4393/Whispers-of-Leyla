using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    //[SerializeField] private Button loadGameButton;

    [Header("Fade")]
    [SerializeField] private Image image;
    private void Start() 
    {
        DisableButtonsDependingOnData();
        Cursor.visible = false;

        AudioManager.instance.LoadVolume();
    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData()) 
        {
            continueGameButton.interactable = false;
            //loadGameButton.interactable = false;
        }

    }

    public void OnNewGameClicked() 
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked() 
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked() 
    {
        StartCoroutine(WaitToLoad());

        //DisableMenuButtons();
        //// save the game anytime before loading new scene 
        //DataPersistenceManager.instance.SaveGame();
        //// load the next scene - which will in turn load the game because of OnSceneLoaded() in the DataPersistenceManager
        //SceneManager.LoadSceneAsync(DataPersistenceManager.instance.GetSavedSceneName());
        //Cursor.visible = false;
    }
    private IEnumerator WaitToLoad()
    {
        //image.DOFade(1, 1f);
        yield return new WaitForSeconds(1.2f);

        DisableMenuButtons();
        // save the game anytime before loading new scene 
        DataPersistenceManager.instance.SaveGame();
        // load the next scene - which will in turn load the game because of OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.GetSavedSceneName());
        Cursor.visible = false;
    }
    private void DisableMenuButtons() 
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
    public void ActivateMenu() 
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }
    public void DeactivateMenu() 
    {
        this.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
