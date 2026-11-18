using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Loading Levels")]
    public int currentLevelIndex = 1; //1 means level 1, 2 means level 2, 3 means level 3. lol. 0 is main menu
    private Coroutine loadToScene;
    [SerializeField] private Animator loadingUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        loadingUI.SetBool("Loading", false);
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //when a scene loads exit from the loadingui state
        loadingUI.SetBool("Loading", false);
    }





    public void LoadLevel()
    {
        if(loadToScene == null)
        {
            loadToScene = StartCoroutine(LoadToScene(currentLevelIndex));
        }
    }

    public void LoadMainMenu()
    {
        if (loadToScene == null)
        {
            loadToScene = StartCoroutine(LoadToScene(0)); //main menu index is 0
        }
    }

    private IEnumerator LoadToScene(int index)
    {
        //set loadingUI's bool, loading, to true
        loadingUI.SetBool("Loading", true);
        yield return new WaitForSeconds(2f);
        //note for next time, do not make a public singleton called scene manager... 
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
       
    }

    
}
