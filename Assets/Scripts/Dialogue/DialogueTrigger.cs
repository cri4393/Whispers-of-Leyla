using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    
    [Header("Emote Animator")]
    [SerializeField] private Animator emoteAnimator;
    private GameObject iris;
    private Animator irisAnim;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [Header("Quest Slot")]
    [SerializeField] private Quest quest;
    [Header("Variable")]
    [SerializeField] private bool isOnlyIris = false;
    private bool collide = false;

    private bool playerInRange;
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);

    }
    private void Start()
    {
        iris = GameObject.FindGameObjectWithTag("Player");
        irisAnim = iris.GetComponent<Animator>();
    }

    private void Update()
    {
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && PlayerMovements.instance.horizontal == 0 && PlayerMovements.instance.LastOnGroundTime > 0 && !isOnlyIris)
        {
            if (InputManager.GetInstance().GetSubmitPressed()  && !DialogueManager.GetInstance().hasStartedDialogue)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, emoteAnimator, quest);
                DialogueManager.GetInstance().hasStartedDialogue = true;
                PlayerMovements.DeactivePlayerControls();
            }
        }
        else if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && isOnlyIris)
        {
            if (collide && !DialogueManager.GetInstance().hasStartedDialogue)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, emoteAnimator, quest);
                DialogueManager.GetInstance().hasStartedDialogue = true;
                collide = false;
                gameObject.SetActive(false);
            }
        }

        else
        {
            visualCue.SetActive(false);
        }

        if (playerInRange && !DialogueManager.GetInstance().hasStartedDialogue)
        {
            visualCue.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Aurelius"))
        {
            playerInRange = true;
            collide = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Aurelius"))
        {
            playerInRange = false;
        }
    }
}
