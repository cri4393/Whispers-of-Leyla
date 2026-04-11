using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public long lastUpdated;
    //public int deathCount;
    public string currentSceneName;
    public Vector3 playerPosition;
    public bool facingRight;
    public bool hasExitTheBox;
    public bool alreadyLimps;
    public bool hasSit;
    public float enemyValue;
    public string swordColor;
    public string hammerColor;
    public string katanaColor;
    public bool secondCameraL1;
    public bool firstTimeFlower;
    public bool drinkFlowerWater;
    public bool activeL9shortCut;
    public bool finishDialogueIrisJump;
    public bool finishJump;
    public bool aureliusCutscene1;
    public bool aureliusCutscene2;
    public bool aureliusCutscene3;
    public bool aureliusCutscene4;
    public bool firstMove;
    public bool secondMove;
    public bool thirdMove;
    public bool fourthMove;
    public bool hasFalling;
    public bool canDoFinalAttack;
    public bool haveDash;
    public string globalVariablesStateJson;

    public int currentWeapon;
    public bool hasShowedColorTutorial;
    public bool hasSavedInNightmare;

    public SerializableDictionary<string, bool> coinsCollected;
    // public SerializableDictionary<Item, bool> itemCollected;
    // public SerializableDictionary<Quest, bool> questCollected;
    // public SerializableDictionary<Memories, bool> memoriesCollected;
    // public SerializableDictionary<string, bool> isOpen;
    // public SerializableDictionary<string, float> enemySpawnValue;
    public List<Quest> questGained;
    public bool prova;
    public bool hasItem1;
    public int souls;
    public int currentHealth;
    public float currentFlask;
    public float valueUltimate;

    //AMULETS DATA
    public bool haveTravellerAmulet;
    public bool haveDashMoreRange;
    public bool haveDashCooldown;

    public string slotTravellerAmulet;
    public string slotEquippedDashMoreRange;
    public string slotDashCooldown;

    //WEAPON EQUIP
    public bool isWeaponEquipUnlocked;

    public string currentMantleEquip;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        // //this.deathCount = 0;
        // playerPosition = new Vector3(0, 0, 0);
        // currentSceneName = "N1Nightmare";
        // facingRight = true;

        // //Narrative Variables
        // hasExitTheBox = false;
        // alreadyLimps = false;
        // hasSit = false;
        // haveDash = false;
        // firstTimeFlower = false;
        // drinkFlowerWater = false;
        // finishDialogueIrisJump = false;
        // finishJump = false;
        // aureliusCutscene1 = false;
        // aureliusCutscene2 = false;
        // aureliusCutscene3 = false;
        // firstMove = false;
        // secondMove = false;
        // thirdMove = false;
        // fourthMove = false;
        // hasFalling = false;
        // canDoFinalAttack = false;
        // hasSavedInNightmare = false;

        this.globalVariablesStateJson = "";

        // hasShowedColorTutorial = false;

        // //Gameplay Variables
        // enemyValue = 0;
        // valueUltimate = 0;
        // swordColor = "";
        // hammerColor = "";
        // katanaColor = "";
        // secondCameraL1 = false;
        // activeL9shortCut = false;
        // coinsCollected = new SerializableDictionary<string, bool>();
        // itemCollected = new SerializableDictionary<Item, bool>();
        // questCollected = new SerializableDictionary<Quest, bool>();
        // memoriesCollected = new SerializableDictionary<Memories, bool>();
        // isOpen = new SerializableDictionary<string, bool>();
        // enemySpawnValue = new SerializableDictionary<string, float>();
        // questGained = new List<Quest>();
        // hasItem1 = false;
        // prova = false;
        // souls = 0;
        // currentHealth = 1;
        // currentFlask = 0;

        // // -Amulets Variables
        // haveTravellerAmulet = false;
        // haveDashMoreRange = false;
        // haveDashCooldown = false;

        // //WeaponEquip
        // isWeaponEquipUnlocked = false;

        // currentMantleEquip = "";
    }

    public int GetPercentageComplete() 
    {
        // figure out how many coins we've collected
        int totalCollected = 0;
        foreach (bool collected in coinsCollected.Values) 
        {
            if (collected) 
            {
                totalCollected++;
            }
        }

        // ensure we don't divide by 0 when calculating the percentage
        int percentageCompleted = -1;
        if (coinsCollected.Count != 0) 
        {
            percentageCompleted = (totalCollected * 100 / coinsCollected.Count);
        }
        return percentageCompleted;
    }

}
