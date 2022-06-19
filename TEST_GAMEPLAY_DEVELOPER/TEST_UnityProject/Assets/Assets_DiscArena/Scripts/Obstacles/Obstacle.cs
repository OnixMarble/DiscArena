using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Image m_HealthBar = null;
    [SerializeField] private ParticleSystem m_HitEffect = null;
    [SerializeField] private GameObject m_Graphics = null;
    [SerializeField] private GameObject m_Canvas = null;
    [SerializeField] private GameEvents m_GameEvents = null;
    [SerializeField] private ObstacleTypes m_ObstacleType = null;
    private float m_Health = 0;
    private float m_MaximumHealth = 0;
    private Renderer[] m_Renderers = null;
    private Collider m_Collider = null;

    private void Awake()
    {
        m_Renderers = m_Graphics.GetComponentsInChildren<Renderer>();
        m_Collider = GetComponent<Collider>();
    }

    private void Start()
    {
        m_Health = m_ObstacleType.Health;
        m_MaximumHealth = m_Health;

        AddBehaviour();
    }

    private void OnEnable()
    {
        m_GameEvents.OnCollisionEvent += CollisionImpact;
    }

    private void OnDisable()
    {
        m_GameEvents.OnCollisionEvent -= CollisionImpact;
    }

    private void AddBehaviour()
    {
        switch (m_ObstacleType.Behaviour)
        {
            case ObstacleBehaviour.RotateLeft:
                gameObject.AddComponent<Rotator>().Initialize(90.0f);
                break;

            case ObstacleBehaviour.RotateRight:
                gameObject.AddComponent<Rotator>().Initialize(-90.0f);
                break;

            default: break;
        }
    }

    private void CollisionImpact(float damage, int ID)
    {
        if (gameObject.GetInstanceID() != ID)
        {
            return;
        }

        m_HitEffect.Play();

        float tempHealth = m_Health - damage;

        if (tempHealth <= 0.0f)
        {
            m_GameEvents.OnObstacleDestroyed();
        }

        ReduceHealth(damage);
    }

    private void ReduceHealth(in float damage)
    {
        m_Health -= damage;
        m_HealthBar.fillAmount = m_Health / m_MaximumHealth;

        if (m_Health > 0)
        {
            return;
        }

        HideGraphics();
        m_Collider.enabled = false;

        if (m_ObstacleType.Type == ObstacleType.Chest)
        {
            m_GameEvents.OnEndGame(true);
        }

        Destroy(transform.parent.gameObject, m_HitEffect.main.duration);
    }

    private void HideGraphics()
    {
        foreach (Renderer renderer in m_Renderers)
        {
            renderer.enabled = false;
        }

        m_Canvas.SetActive(false);
    }
}
