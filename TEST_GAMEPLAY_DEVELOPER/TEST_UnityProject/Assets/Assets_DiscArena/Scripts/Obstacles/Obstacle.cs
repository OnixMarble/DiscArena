using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Image m_HealthBar = null;
    private float m_Health = 100;
    private readonly float m_MaximumHealth = 100;
    private bool m_IsSimulationObject = false;

    private void Start()
    {
        m_IsSimulationObject = gameObject.CompareTag("Simulation");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If this is a simulation object (i.e. object in the simulation scene), then simply return
        if (m_IsSimulationObject)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            ReduceHealth();
        }
    }

    private void ReduceHealth()
    {
        m_Health -= 50.0f;
        m_HealthBar.fillAmount = m_Health / m_MaximumHealth;

        if (m_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
