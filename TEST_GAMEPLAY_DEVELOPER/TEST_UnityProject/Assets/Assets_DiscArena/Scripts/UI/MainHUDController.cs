using UnityEngine;
using UnityEngine.UI;

public class MainHUDController : MonoBehaviour
{
    [SerializeField] private InputReader m_InputReader = null;
    [SerializeField] private GameEvents m_GameEvents = null;
    [SerializeField] private Text m_DiscsLeftText = null;
    [SerializeField] private Text m_WinLoseText = null;
    [SerializeField] private Button m_ClassicDiscButton = null;
    [SerializeField] private Button m_SprinterDiscButton = null;
    private Scaler m_Scaler = null;

    private void Awake()
    {
        m_Scaler = GetComponentInChildren<Scaler>();
    }

    private void OnEnable()
    {
        m_GameEvents.OnUpdateTotalDiscsEvent += OnUpdateTotalDiscs;
        m_GameEvents.OnNewTurnEvent += OnNewTurn;
        m_GameEvents.OnGameEndedEvent += OnGameEnded;

        m_ClassicDiscButton.onClick.AddListener(() => { OnSwapDiscButtonPressed(GameEvents.DiscTypes.Classic); });
        m_SprinterDiscButton.onClick.AddListener(() => { OnSwapDiscButtonPressed(GameEvents.DiscTypes.Sprinter); });
    }

    private void OnDisable()
    {
        m_GameEvents.OnUpdateTotalDiscsEvent -= OnUpdateTotalDiscs;
        m_GameEvents.OnNewTurnEvent -= OnNewTurn;
        m_GameEvents.OnGameEndedEvent -= OnGameEnded;

        m_ClassicDiscButton.onClick.RemoveListener(() => { OnSwapDiscButtonPressed(GameEvents.DiscTypes.Classic); });
        m_SprinterDiscButton.onClick.RemoveListener(() => { OnSwapDiscButtonPressed(GameEvents.DiscTypes.Sprinter); });
    }

    private void OnSwapDiscButtonPressed(GameEvents.DiscTypes newDisc)
    {
        m_GameEvents.OnSwapDiscs(newDisc);
        m_InputReader.ToggleGameplayInput(true);
        ToggleButtons(false);
    }

    private void OnNewTurn()
    {
        m_InputReader.ToggleGameplayInput(false);
        ToggleButtons(true);
    }

    private void ToggleButtons(in bool toggle)
    {
        m_ClassicDiscButton.enabled = toggle;
        m_SprinterDiscButton.enabled = toggle;
        m_Scaler.enabled = toggle;
    }

    private void OnGameEnded(bool winner)
    {
        if (m_WinLoseText.gameObject.activeInHierarchy)
        {
            return;
        }

        m_WinLoseText.gameObject.SetActive(true);
        m_WinLoseText.text = winner ? "Victory!" : "Defeat";
    }

    private void OnUpdateTotalDiscs(int totalDiscs)
    {
        m_DiscsLeftText.text = totalDiscs.ToString() + " DISCS LEFT";
    }
}
