using UnityEngine;

public class CreatureController : MonoBehaviour
{

    [Header("Creature State")]
    public CreatureState creatureState;
    [Header("Animator")]
    [SerializeField] private Animator anim;
    [Header("Creature Positions")]
    [SerializeField] private Transform[] creaturePositions;
    [SerializeField] private Transform[] windowPositions, closetPositions, doorPositions, ventPositions;

    public enum CreatureState
    {
        thinking,
        window,
        closet,
        door,
        vent


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(creatureState)
        {
            case CreatureState.thinking:
                transform.position = creaturePositions[0].position;
                break;
            case CreatureState.window:
                transform.position = creaturePositions[1].position;
                break;
            case CreatureState.closet:
                transform.position = creaturePositions[2].position;
                break;
            case CreatureState.door:
                transform.position = creaturePositions[3].position;
                break;
            case CreatureState.vent:
                transform.position = creaturePositions[4].position;
                break;
        }
    }

    void Thinking()
    {

    }

    void Window()
    {

    }

    void Closet()
    {


    }

    void Door()
    {


    }

    void Vent()
    {

    }


    /*Psuedo Code For Creature Controller
     * Little note, I know it is not the best practice for puting psuedo code at the bottom of the actual code file however I am making this game in a short time and I want to kind of show this code off :D
     * 
     * Thinking: Creature will determine the best approach to the player depending on what the player is looking at and the creatures previous approach and possible approaches
     What does this mean?
    well the opened approaches will have priority, so if the closet is opened it will try and go there, next priority is the closer aproaches, however creature can not go to the same approach twice in a row
    
     */
}
