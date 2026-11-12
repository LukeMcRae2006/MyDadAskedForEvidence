using UnityEngine;

public class SceneManager : MonoBehaviour
{

    [Header("Scene Time References")]
    [SerializeField] private ClockScript clock;
    private float timeCount = 0;
    private int timeIndex = 0;
    [SerializeField] private int hour = 3;
    [SerializeField] private float inGameMinute = 45f;

    [Header("Victory")]
    public bool hourCompleted = false;
    [SerializeField] private GameObject victoryTimeline;

    [Header("Game Over References")]
    [SerializeField] private GameObject gameOverTimeline;



    public static SceneManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    void Update()
    {
        SceneRuntime();
    }

    void SceneRuntime()
    {
        SceneTime();
    }

    void SceneTime()
    {
        Debug.Log("TimeCount: " + timeCount);
        //The entire level you must survive 6 minutes
        if(timeCount < inGameMinute) //check if timeCount is less than an in game minute
        {
            timeCount += Time.deltaTime;
        }
        else //increase the timeIndex
        {
            //we hit 10 seconds, revert count
            timeCount = 0;
            //check if we should go up an hour or just 10 minutes
            if (timeIndex == 5) //this checks if next update should be an hour
            {
                timeIndex = 0;
                hour += 1;
                AchieveVictory();
            }else
            {
                timeIndex += 1;
            }
           
            //set new time as the clock text
            string newTime = hour + ":" + timeIndex + "0";
            clock.SetClockTime(newTime);
            
        }

        

    }

    private void AchieveVictory()
    {
        hourCompleted = true;
        PlayerController.Instance.playerState = PlayerController.PlayerState.GameOver;
        victoryTimeline.SetActive(true);
    }


    public void GameOver()
    {
        gameOverTimeline.SetActive(true);
        PlayerController.Instance.gameObject.SetActive(false);
    }
}
