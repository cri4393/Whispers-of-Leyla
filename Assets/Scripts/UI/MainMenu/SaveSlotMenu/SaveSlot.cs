using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI deathCountText;
    [SerializeField] private TextMeshProUGUI zones;

    [Header("Biomes")]
    [SerializeField] private GameObject Ruins;
    [SerializeField] private GameObject Forest;
    [SerializeField] private GameObject FireflyCaves;

    [Header("Clear Data Button")]
    [SerializeField] private Button clearButton;
    public bool hasData { get; private set; } = false;

    private Button saveSlotButton;

    private void Awake() 
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data) 
    {
        // there's no data for this profileId
        if (data == null) 
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
        }
        // there is data for this profileId
        else 
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);

            percentageCompleteText.text = data.GetPercentageComplete() + "% COMPLETE";

            if(data.currentSceneName == "L1IrisRuins")
            {
                Ruins.SetActive(true);
                Forest.SetActive(false);
                FireflyCaves.SetActive(false);
                zones.text = "Floreal Caves";
            }
            if(data.currentSceneName == "F6Forest")
            {
                Ruins.SetActive(false);
                Forest.SetActive(false);
                FireflyCaves.SetActive(true);
                zones.text = "Firefly Caves";
            }
            //deathCountText.text = "DEATH COUNT: " + data.deathCount;
        }
    }

    public string GetProfileId() 
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearButton.interactable = interactable;
    }
}