using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Image m_HealthBar = null;
    [SerializeField] private ParticleSystem m_HitEffect = null;
    [SerializeField] private GameObject m_Graphics = null;
    private float m_Health = 100;
    private readonly float m_MaximumHealth = 100;
    private bool m_IsSimulationObject = false;
    private Renderer[] m_Renderers = null;
    private Image[] m_Images = null;
    private Collider m_Collider = null;

    private void Start()
    {
        m_IsSimulationObject = gameObject.CompareTag("Simulation");

        if (!m_IsSimulationObject)
        {
            m_Renderers = m_Graphics.GetComponentsInChildren<Renderer>();
            m_Images = GetComponentsInChildren<Image>();
            m_Collider = GetComponent<Collider>();
        }
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
            m_HitEffect.Play();
            ReduceHealth();
        }
    }

    private void ReduceHealth()
    {
        m_Health -= 50.0f;
        m_HealthBar.fillAmount = m_Health / m_MaximumHealth;

        if (m_Health <= 0)
        {
            HideGraphics();
            m_Collider.enabled = false;
            Destroy(gameObject, m_HitEffect.main.duration);
        }
    }

    private void HideGraphics()
    {
        foreach (Renderer renderer in m_Renderers)
        {
            renderer.enabled = false;
        }

        foreach (Image image in m_Images)
        {
            image.enabled = false;
        }
    }
}
