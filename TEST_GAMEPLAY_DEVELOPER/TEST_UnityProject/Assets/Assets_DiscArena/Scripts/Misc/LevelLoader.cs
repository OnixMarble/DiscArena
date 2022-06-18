using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameEvents m_GameEvents = null;
    private WaitForSeconds m_DelayInSeconds = null;

    private void Awake()
    {
        m_DelayInSeconds = new WaitForSeconds(2.0f);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        m_GameEvents.OnGameEndedEvent += OnEndGameState;
    }
    private void OnDisable()
    {
        m_GameEvents.OnGameEndedEvent += OnEndGameState;
    }

    private void OnEndGameState(bool winCondition)
    {
        StartCoroutine(LoadLevel(winCondition));
    }

    private IEnumerator LoadLevel(bool winCondition)
    {
        yield return m_DelayInSeconds;

        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (winCondition && currentLevelIndex == 1)
        { 
            // Keep swapping between the 2 scenes for now
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(winCondition ? currentLevelIndex + 1 : currentLevelIndex);
        }
    }
}
