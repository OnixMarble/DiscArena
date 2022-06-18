using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Image m_HealthBar = null;
    [SerializeField] private ParticleSystem m_HitEffect = null;
    [SerializeField] private GameObject m_Graphics = null;
    [SerializeField] private GameEvents m_GameEvents = null;
    private float m_Health = 100;
    private readonly float m_MaximumHealth = 100;
    private Renderer[] m_Renderers = null;
    private Image[] m_Images = null;
    private Collider m_Collider = null;

    private void Awake()
    {
        m_Renderers = m_Graphics.GetComponentsInChildren<Renderer>();
        m_Images = GetComponentsInChildren<Image>();
        m_Collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        m_GameEvents.OnCollisionEvent += OnCollisionImpact;
    }

    private void OnDisable()
    {
        m_GameEvents.OnCollisionEvent -= OnCollisionImpact;
    }

    private void OnCollisionImpact(float damage, int ID)
    {
        if (ID != gameObject.GetInstanceID())
        {
            return;
        }

        m_HitEffect.Play();

        float tempHealth = m_Health - damage;

        if (tempHealth <= 0.0f)
        {
            // TODO: If hit results in this obstacle's death, then allow disc to power through the obstacle
        }

        ReduceHealth();
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
