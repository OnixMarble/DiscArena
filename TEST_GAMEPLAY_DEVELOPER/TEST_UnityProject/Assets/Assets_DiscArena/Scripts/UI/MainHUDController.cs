using UnityEngine;
using UnityEngine.UI;

public class MainHUDController : MonoBehaviour
{
    [SerializeField] private GameEvents m_GameEvents = null;
    [SerializeField] private Text m_DiscsLeftText = null;
    [SerializeField] private Text m_WinLoseText = null;

    private void OnEnable()
    {
        m_GameEvents.OnUpdateTotalDiscsEvent += UpdateTotalDiscsText;
        m_GameEvents.OnGameEndedEvent += GameEnded;
    }


    private void OnDisable()
    {
        m_GameEvents.OnUpdateTotalDiscsEvent -= UpdateTotalDiscsText;
        m_GameEvents.OnGameEndedEvent -= GameEnded;
    }

    private void GameEnded(bool winner)
    {
        m_WinLoseText.gameObject.SetActive(true);
        m_WinLoseText.text = winner ? "Victory!" : "Defeat";
    }

    private void UpdateTotalDiscsText(int totalDiscs)
    {
        m_DiscsLeftText.text = totalDiscs.ToString() + " DISCS LEFT";
    }
}
