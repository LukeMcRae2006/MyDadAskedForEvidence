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
}
