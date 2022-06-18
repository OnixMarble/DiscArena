using UnityEngine;

public class DiscController : MonoBehaviour
{
    [SerializeField] private InputReader m_InputReader = null;
    [SerializeField] private GameEvents m_GameEvents = null;
    private Rigidbody m_Rigidbody = null;
    private DiscProjectile m_DiscShoot = null;
    private bool m_ShotDisc = false;

    private void Awake()
    {
        m_DiscShoot = GetComponentInChildren<DiscProjectile>();
        m_Rigidbody = m_DiscShoot.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SetupCallbacks(true);
    }

    private void OnDisable()
    {
        SetupCallbacks(false);
    }

    private void SetupCallbacks(in bool bind)
    {
        if (bind)
        {
            m_InputReader.OnShootEvent += Shoot;
            m_DiscShoot.OnCollisionEvent += CollisionDetected;
        }
        else
        {
            m_InputReader.OnShootEvent -= Shoot;
            m_DiscShoot.OnCollisionEvent -= CollisionDetected;
        }
    }

    private void FixedUpdate()
    {
        HandleDiscShot();
    }

    private void Shoot(Vector2 touchPosition)
    {
        if (m_ShotDisc)
        {
            return;
        }

        m_DiscShoot.ShootDisc(touchPosition);
        m_ShotDisc = true;
    }

    private void HandleDiscShot()
    {
        if (!m_ShotDisc)
        {
            return;
        }

        ResetPositionWhenStopped(transform.position);
    }

    private void ResetPositionWhenStopped(in Vector3 returnPosition)
    {
        float speed = m_Rigidbody.velocity.magnitude;
        float stoppedThreshold = 2.0f;

        if (speed == 0 || m_Rigidbody.velocity == Vector3.zero)
        {
            // TODO: This is a hack, remove later!
            return;
        }

        if (speed <= stoppedThreshold)
        {
            m_Rigidbody.position = returnPosition;
            m_ShotDisc = false;
            StartNewTurn();
        }
    }

    private void StartNewTurn()
    {
        m_GameEvents.OnNewTurn();
    }

    private void CollisionDetected(float damage, int collisionID)
    {
        m_GameEvents.OnCollision(damage, collisionID);
    }
}
