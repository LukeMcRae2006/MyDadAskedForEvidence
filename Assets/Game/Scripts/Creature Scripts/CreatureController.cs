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
    //Creature position progess is the progress the creature has made to the player
    //for the window there is nothing, the creature is at the window, and if the player does not see in time, the creature attacks
    //For the closet, door, and vent, the creature will check if the player can see it, if not it progress through the positions;
    [Range(1,2)]
    [SerializeField] private int windowProgress, ventProgress, closetProgress, doorProgress;
    [Header("Creature Actions")]
    //reference to creatures last choice
    private CreatureState lastCreatureChoice = CreatureState.vent;
    [Header("Creature Thinking Attributes (Difficult Settings)")]
    [SerializeField] private float firstThinkingDelaySetter, maxThinkingDelaySetter;
    [Range(0, 100)]
    [SerializeField] private int windowProb, closetProb, doorProb, ventProb; //probs will start based off of the sum of the previous prob(s)
    private float thinkingDelay = 0;
    [SerializeField] private float approachDelaySetter;
   [SerializeField] private float approachDelay;
    private bool didOpenCheck = false;
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
        //set our delay timers to their setter values
        thinkingDelay = firstThinkingDelaySetter;
        approachDelay = approachDelaySetter;
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

    #region Creature Approach And Thinking

    void Thinking()
    {
        transform.position = thinkingPositions[1].position;
        //check if thinking delay is done.
        if(thinkingDelay > 0)
        {
            thinkingDelay -= Time.deltaTime;
            return;
        }

        //now lets check best approaches and see if any approach is open

        if(RoomManager.Instance.isWindowOpen == true)
        {
            //window is open, increase window approach odds
            ChangeApproachProbability(1);
        }
        else if (RoomManager.Instance.isClosetOpen == true)
        {
            //closet is open increase closet approach odds
            ChangeApproachProbability(2);
        }
        else if (RoomManager.Instance.isDoorOpen == true)
        {
            //door is open increase door approach odds
            ChangeApproachProbability(3);
        }
        else if (RoomManager.Instance.isVentOpen == true)
        {
            //vent is open increase vent approach odds
            ChangeApproachProbability(4);
        }
        else
        {
            //nothing is open so set odds to normal
            ChangeApproachProbability(0);
        }

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

#endregion

    #region Approach Probability
    void ChangeApproachProbability(int probabilityIndex)
    {
        //probabilities will go from left to right with indexes;
        //So: 1 = windowProb, 2 = closetProb, 3 = doorProb, 4 = ventProb
        //Before creature approaches it checks if there are any approaches open, if so there is a higher probability of that
        //we check if vent is open last to make it easier for the player, if we want to increase difficulty we can check before door or even closet

        
         /* Original indexs on inspector slider are shown below and are inclusive
         * window: 0-20
         * closet: 21-53
         * door: 54-85
         * vent: 85-100
         */
        

        if(probabilityIndex == 1)
        {
            //index is on window
            windowProb = 60; //60
            closetProb = 73; //13
            doorProb = 86; //13
            ventProb = 100; //14
        }
        else if(probabilityIndex == 2)
        {
            //index is on closet
            windowProb = 13; //13
            closetProb = 73; //60
            doorProb = 85; //12
            ventProb = 100;//15
        }
        else if(probabilityIndex == 3)
        {
            //index is on door
            windowProb = 13;//13
            closetProb = 26;//13
            doorProb = 86;// 60
            ventProb = 100; //14
        }
        else if(probabilityIndex == 4)
        {
            //index is on vent
            windowProb = 15; //15
            closetProb = 48;//33
            doorProb = 80;//32
            ventProb = 100;//20
        }
        else
        {
            //no index selected revert back to original probabilities
            windowProb = 20; //20
            closetProb = 53; //33
            doorProb = 85; //32
            ventProb = 100; //15
        }
    }

    #endregion

    #region Creature's Approach And Actions
    public void CreatureDetected()
    {
        //creature is detected! Retreat!
        Retreat();
        thinkingDelay = Random.Range(5f,maxThinkingDelaySetter); //thinking delay is random between max thinking delay and a base of 5 seconds
        creatureState = CreatureState.thinking;

    }

    

    void Window()
    {
        
        if (didOpenCheck == false)//check to see if we already checked progress and if approach is being seen by player
        {
            if (!RoomManager.Instance.isWindowOpen)
            {
                windowScript.Interact();
                windowProgress = 1;
            }
            else
            {
                windowProgress = 2;
            }
            
        }


        if (CanIncreaseApproach())
        {
            windowProgress += 1;
        }


        //Window Progress
        if (windowProgress == 1)
        {
            PlayAnimation("ANI_HauntWindow_1");
            transform.position = windowPositions[0].position;
        }
        else
        {
            //JUMPSCARE THE PLAYER
            CapturePlayer();

        }

        didOpenCheck = true;
        

    }

    void Closet()
    {
        if (didOpenCheck == false )//check to see if we already checked progress and if approach is being seen by player
        {
            if (RoomManager.Instance.isClosetOpen == false)
            {
                closetScript.Interact();
                closetProgress = 1;

            }
            else
            {
                closetProgress = 2;
            }
            
        }

        if (CanIncreaseApproach())
        {
            closetProgress += 1;
        }


        if (closetProgress == 1)
        {
            PlayAnimation("ANI_HauntCloset_1");
            transform.position = closetPositions[0].position;
        }
        else if(closetProgress == 2)
        {
            PlayAnimation("ANI_HauntBed_1");
            transform.position = closetPositions[1].position;
            

        }
        else
        {
            CapturePlayer();
        }

            didOpenCheck = true;

        

       

    }

    void Door()
    {
        if(didOpenCheck == false)//check to see if we already checked progress
        { 
        if (RoomManager.Instance.isDoorOpen == false) //check if approach is being seen by player and if door is open
        {
                doorScript.Interact();
                doorProgress = 1;
        }
        else 
        {
                    doorProgress = 2;
                
        }
            

        }


        if (CanIncreaseApproach())
        {

            doorProgress += 1;
        }
        

        if (doorProgress == 1)
        {
            PlayAnimation("ANI_HauntDoor_1");
            transform.position = doorPositions[0].position;
        }
        else if(doorProgress == 2)  
        {
            PlayAnimation("ANI_HauntBed_1");
            transform.position = doorPositions[1].position;
            

        }else
        {
            CapturePlayer();
        }

        didOpenCheck = true;

       
    }

    void Vent()
    {
        


        ventScript.Interact();


        if (CanIncreaseApproach())
        {
            ventProgress += 1;
        }

        if(ventProgress == 1)
        {
            PlayAnimation("ANI_HauntVent_1");
            transform.position = ventPositions[0].position;
        }
        else
        {
            CapturePlayer();
        }


    }

    void Retreat() //Retreat handles the approach logic when the creature is detected
    {
        
        PlayAnimation("ANI_Retreat");
        transform.position = thinkingPositions[0].position;
        //reset approach progress since this can change any time and is dependent on player
        ventProgress = 1;
        windowProgress = 1;
        doorProgress = 1;
        closetProgress = 1;
        //reset approach delay
        approachDelay = approachDelaySetter;
        //set did open check = false
        didOpenCheck = false;
    }

   

    bool CanIncreaseApproach()
    {
        //check if the creature is seen by player, if not increase approach
       
            if (approachDelay > 0)
            {
                approachDelay -= Time.deltaTime;
                return false;
            }
            else
            {
                approachDelay = approachDelaySetter;
                return true;
                
            }
        
      
        

    }

    #endregion

    void PlayAnimation(string animName)
    {
        anim.Play(animName);
    }



    void CapturePlayer()
    {
        Debug.Log("JumpScare The Player");
        RoomManager.Instance.GameOver();
    }
   
    /*Psuedo Code For Creature Controller
     * Little note, I know it is not the best practice for puting psuedo code at the bottom of the actual code file however I am making this game in a short time and I want to kind of show this code off :D
     * 
     * Thinking: Creature will determine the best approach to the player depending on what the player is looking at and the creatures previous approach and possible approaches
     What does this mean?
    well the opened approaches will have priority, so if the closet is opened it will try and go there, next priority is the closer aproaches, however creature can not go to the same approach twice in a row
    
     */

    /*Psuedo code for increasing approach
     * If the creature is currently in position, check to see if the player is looking at them, if not increase approach.
     * Increasing approach moves the current position index up by one and 
     * then goes back to checking
     * 
     * 
     * 
     * 
     */
}
