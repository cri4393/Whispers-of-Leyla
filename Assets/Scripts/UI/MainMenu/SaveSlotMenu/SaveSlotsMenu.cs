using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SaveSlotsMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfermationPopupMenu confermationPopupMenu;
    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenuObj;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    [Header("Fade")]
    [SerializeField] private Image image;

    private void Awake() 
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }
    public void OnSaveSlotClicked(SaveSlot saveSlot) 
    {
        // disable all buttons
        DisableMenuButtons();

        // case - loading game
        if(isLoadingGame)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            //SaveGameAndLoadScene();
            LoadScene();
        }
        // case - new game, but the save slot has data
        else if(saveSlot.hasData)
        {
            StartCoroutine(WaitToLoadHasData(saveSlot));

            //DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            //LoadScene();
        }
        // case - new game, and the save slot has no data
        else
        {
            StartCoroutine(WaitToLoadNoData(saveSlot));

            //DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            //DataPersistenceManager.instance.NewGame();
            //SaveGameAndLoadScene();
        }
    }

    private void SaveGameAndLoadScene()
    {
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();

        // load the scene 
        SceneManager.LoadSceneAsync("N1Nightmare");
        LoadLevelASync("N1Nightmare");
    }

    private void LoadScene()
    {
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();

        //load the scene
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.GetSavedSceneName());
        Cursor.visible = false;
        //LoadLevelASync(SceneName.instance.currentScene);
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confermationPopupMenu.ActivateMenu(
            "Are you sure you want to delete this saved data?",
            // function to execute if we select "yes"
            () => {
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
                ActivateMenu(isLoadingGame);
            },
            // function to execute if we select "cancel"
            () => {
                ActivateMenu(isLoadingGame);
            }
        );
    }

    public void OnBackClicked() 
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame) 
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        // set mode
        this.isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        // ensure the back button is enabled when we activate the menu
        backButton.interactable = true;

        // loop through each save slot in the UI and set the content appropriately
        GameObject firstSelected = backButton.gameObject;
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame) 
            {
                saveSlot.SetInteractable(false);
            }
            else 
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        // set the first selected button
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu() 
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons() 
    {
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
    private IEnumerator WaitToLoadHasData(SaveSlot saveSlot)
    {
        //image.DOFade(1, 1f);
        yield return new WaitForSeconds(1.2f);
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        LoadScene();
    }
    private IEnumerator WaitToLoadNoData(SaveSlot saveSlot)
    {
        //image.DOFade(1, 1f);
        yield return new WaitForSeconds(1.2f);
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        DataPersistenceManager.instance.NewGame();
        SaveGameAndLoadScene();
    }

    public void LoadLevelBtn(string levelToLoad)
    {
        mainMenuObj.SetActive(false);
        loadingScreen.SetActive(true);

        //Run the A sync 
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        
        while (!loadOperation.isDone)
        {
            float progressionValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressionValue;
            yield return null;
        }
    }
}
