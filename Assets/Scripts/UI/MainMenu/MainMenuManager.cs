using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.UI;
using TMPro;
public class MainMenuManager : MonoBehaviour
{
    [Header("PressAnyButton")]
    public LeanTweenType anyButton;
    [SerializeField] private GameObject pressAnyButtonMenu;
    [SerializeField] private GameObject pressAnyButtonText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject arrow;

    [Header("ButtonGO")]
    [SerializeField] private GameObject newGameGO;
    [SerializeField] private GameObject continueGO;
    [SerializeField] private GameObject optionsGO;
    [SerializeField] private GameObject quitGO;
    private Button quitButton;
    private Button newGameButton;
    private Button continueButton;
    private Button optionsButton;

    [SerializeField] private CinemachineCamera cameraMainMenu;

    private bool havePressedAnyButton;

    private void Start()
    {
        newGameButton = newGameGO.GetComponent<Button>();
        continueButton = continueGO.GetComponent<Button>();
        optionsButton = optionsGO.GetComponent<Button>();
        quitButton = quitGO.GetComponent<Button>();
    }
    private void UpdateTextAlpha(float alpha)
    {
        Color color = pressAnyButtonText.GetComponent<TextMeshProUGUI>().color;
        color.a = alpha;
        pressAnyButtonText.GetComponent<TextMeshProUGUI>().color = color;
    }
    private void Update()
    {
        if (InputManager.GetInstance().PressedAnyButton() && !havePressedAnyButton)
        {
            StartCoroutine(Pressed());
        }
    }

    private IEnumerator Pressed()
    {
        havePressedAnyButton = true;
        //LeanTween.scale(pressAnyButtonText, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
        LeanTween.scale(pressAnyButtonText, new Vector3(1.3f, 1.3f, 1.3f), 0.2f).setEase(anyButton);
        LeanTween.scale(pressAnyButtonText, new Vector3(1f, 1f, 1f), 0.2f).setEase(anyButton);
        yield return new WaitForSeconds(.1f);
        //LeanTween.scale(pressAnyButtonText, new Vector3(0f, 0f, 0f), 0.5f);
        LeanTween.value(pressAnyButtonText, 1, 0, .2f).setOnUpdate(UpdateTextAlpha);
        yield return new WaitForSeconds(2f);
        cameraMainMenu.enabled = true;
        yield return new WaitForSeconds(3f);
        mainMenu.SetActive(true);
        //LeanTween.scale(newGameGO, new Vector3(1.3f, 1.3f, 1.3f), 0.5f).setDelay(.5f);
        //LeanTween.scale(continueGO, new Vector3(.94f, .94f, .94f), 0.5f).setDelay(.1f);
        //LeanTween.scale(optionsGO, new Vector3(.91f, .91f, .91f), 0.5f).setDelay(.7f);
        //LeanTween.scale(arrow, new Vector3(1.3f, 1.3f, 1.3f), 0.5f).setDelay(.7f);
        //LeanTween.scale(newGameGO, new Vector3(.18f, .18f, .18f), 0.5f).setDelay(.1f);
        yield return new WaitForSeconds(2.5f);
        newGameButton.interactable = true;
        continueButton.interactable = true;
        optionsButton.interactable = true;
        quitButton.interactable = true;
    }
}
