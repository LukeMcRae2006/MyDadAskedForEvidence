using UnityEngine;

public class CreatureController : MonoBehaviour
{

    [Header("Creature State")]
    public CreatureState creatureState;
    [Header("Animator")]
    [SerializeField] private Animator anim;
    [Header("Creature Positions")]
    [SerializeField] private Transform[] thinkingPositions; // 0 = retreat, 1 = thinking
    [SerializeField] private Transform[] windowPositions, closetPositions, doorPositions, ventPositions;
    [Header("Creature Actions")]
    //reference to creatures last choice
    private CreatureState lastCreatureChoice = CreatureState.vent;
    [Header("Creature Thinking Attributes")]
    [SerializeField] private float thinkingDelaySetter, thinkingDelayRetreatSetter;
    [Range(0, 100)]
    [SerializeField] private int windowProb, closetProb, doorProb, ventProb; //probs will start based off of the sum of the previous prob(s)
    private float thinkingDelay = 0;
    [Header("Creature Approach References")]
    [SerializeField] private WindowScript windowScript;
    [SerializeField] private VentScript ventScript;
    [SerializeField] private DoorScript doorScript;
    [SerializeField] private ClosetScript closetScript;
    



    //Instance
    public static CreatureController Instance;

    public enum CreatureState
    {
        thinking,
        window,
        closet,
        door,
        vent,
        retreat


    }

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thinkingDelay = thinkingDelaySetter;
    }

    // Update is called once per frame
    void Update()
    {
        switch(creatureState)
        {
            case CreatureState.thinking:
                Thinking();
                break;
            case CreatureState.window:
                Window();
                break;
            case CreatureState.closet:
                Closet();
                break;
            case CreatureState.door:
                Door();
                break;
            case CreatureState.vent:
                Vent();
                break;
            case CreatureState.retreat:
                Retreat();
                break;
        }
    }

    void Thinking()
    {
        transform.position = thinkingPositions[1].position;
        //check if thinking delay is done.
        if(thinkingDelay > 0)
        {
            thinkingDelay -= Time.deltaTime;
            return;
        }

        //now lets check best approaches

        int choice = Random.Range(0, 100);
        Debug.Log("creature Random Number Generated: " + choice);
        if (choice <= windowProb)
        {
            ChooseCreatureApproachChoice(CreatureState.window);
        }
        else if(choice <= closetProb)
        {
            ChooseCreatureApproachChoice(CreatureState.closet);
        }
        else if (choice <= doorProb)
        {
            ChooseCreatureApproachChoice(CreatureState.door);
        }
        else
        {
            ChooseCreatureApproachChoice(CreatureState.vent);
        }

      
      

      
        
    }


    void ChooseCreatureApproachChoice(CreatureState newState)
    {
        if(newState == lastCreatureChoice)
        {
            creatureState = CreatureState.thinking;
            return;
        }
        creatureState = newState;
        lastCreatureChoice = creatureState;
    }


    public void CreatureDetected()
    {
        //creature is detected! Retreat!
        thinkingDelay = thinkingDelayRetreatSetter;
        creatureState = CreatureState.thinking;

    }

    

    void Window()
    {
        PlayAnimation("ANI_HauntWindow_1");
        transform.position = windowPositions[0].position;
        if(RoomManager.Instance.isWindowOpen == false)
        {
            windowScript.Interact();
        }
    }

    void Closet()
    {
        PlayAnimation("ANI_HauntCloset_1");
        transform.position = closetPositions[0].position;
        if (RoomManager.Instance.isClosetOpen == false)
        {
            closetScript.Interact();
        }

    }

    void Door()
    {
        PlayAnimation("ANI_HauntDoor_1");
        transform.position = doorPositions[0].position;
        if (RoomManager.Instance.isDoorOpen == false)
        {
            doorScript.Interact();
        }
    }

    void Vent()
    {
        PlayAnimation("ANI_HauntVent_1");
        transform.position = ventPositions[0].position;
        ventScript.Interact();
    }

    void Retreat()
    {
        PlayAnimation("ANI_Retreat");
        transform.position = thinkingPositions[0].position;
    }

    void PlayAnimation(string animName)
    {
        anim.Play(animName);
    }

   
    /*Psuedo Code For Creature Controller
     * Little note, I know it is not the best practice for puting psuedo code at the bottom of the actual code file however I am making this game in a short time and I want to kind of show this code off :D
     * 
     * Thinking: Creature will determine the best approach to the player depending on what the player is looking at and the creatures previous approach and possible approaches
     What does this mean?
    well the opened approaches will have priority, so if the closet is opened it will try and go there, next priority is the closer aproaches, however creature can not go to the same approach twice in a row
    
     */
}
