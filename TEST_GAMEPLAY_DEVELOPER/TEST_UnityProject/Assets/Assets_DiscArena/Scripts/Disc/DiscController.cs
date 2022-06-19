using System.Collections.Generic;
using UnityEngine;

public class DiscController : MonoBehaviour
{
    [SerializeField] private InputReader m_InputReader = null;
    [SerializeField] private GameEvents m_GameEvents = null;
    [SerializeField] private int m_TotalDiscs = 5;
    [SerializeField] private List<DiscData> m_DiscScriptableObjects = new List<DiscData>();
    private Rigidbody m_Rigidbody = null;
    private DiscProjectile m_DiscProjectile = null;
    private bool m_ShotDisc = false;
    private Vector3 m_CurrentVelocity = Vector3.zero;

    private void Awake()
    {
        m_DiscProjectile = GetComponentInChildren<DiscProjectile>();
        m_Rigidbody = m_DiscProjectile.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_GameEvents.OnUpdateDiscsCount(m_TotalDiscs);
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
            m_InputReader.OnTouchEndEvent += OnShoot;
            m_DiscProjectile.OnCollisionEvent += OnCollisionDetected;
            m_GameEvents.OnObstacleDestroyedEvent += OnCollisionDestroyed;
            m_GameEvents.OnSwapDiscsEvent += OnSwapDiscs;
        }
        else
        {
            m_InputReader.OnTouchEndEvent -= OnShoot;
            m_DiscProjectile.OnCollisionEvent -= OnCollisionDetected;
            m_GameEvents.OnObstacleDestroyedEvent -= OnCollisionDestroyed;
            m_GameEvents.OnSwapDiscsEvent -= OnSwapDiscs;
        }
    }

    private void OnSwapDiscs(GameEvents.DiscTypes newDisc)
    {
        switch (newDisc)
        {
            case GameEvents.DiscTypes.Classic:
                m_DiscProjectile.SetDiscData(m_DiscScriptableObjects[(int)GameEvents.DiscTypes.Classic]);
                break;

            case GameEvents.DiscTypes.Sprinter:
                m_DiscProjectile.SetDiscData(m_DiscScriptableObjects[(int)GameEvents.DiscTypes.Sprinter]);
                break;

            default: break;
        }
    }

    private void FixedUpdate()
    {
        m_CurrentVelocity = m_Rigidbody.velocity;

        HandleDiscShot();
    }

    private void OnShoot(Vector2 touchPosition)
    {
        if (m_ShotDisc)
        {
            return;
        }

        m_DiscProjectile.ShootDisc(touchPosition);
        m_ShotDisc = true;
        --m_TotalDiscs;

        m_GameEvents.OnUpdateDiscsCount(m_TotalDiscs);
    }

    private void HandleDiscShot()
    {
        if (!m_ShotDisc)
        {
            return;
        }

        ResetPositionWhenStopping();
    }

    private void ResetPositionWhenStopping()
    {
        float speed = m_Rigidbody.velocity.magnitude;
        float stoppedThreshold = 2.0f;

        if (speed == 0)
        {
            return;
        }

        if (speed <= stoppedThreshold)
        {
            StartNewTurn();
        }
    }

    private void StartNewTurn()
    {
        if (m_TotalDiscs == 0)
        {
            m_GameEvents.OnEndGame(false);
            m_Rigidbody.isKinematic = true;
            enabled = false;
            return;
        }

        m_Rigidbody.position = transform.position;
        m_Rigidbody.velocity = Vector3.zero;
        m_ShotDisc = false;

        m_GameEvents.OnNewTurn();
    }

    private void OnCollisionDetected(float damage, int collisionID)
    {
        m_GameEvents.OnCollision(damage, collisionID);
    }

    private void OnCollisionDestroyed()
    {
        // If this disc destroyed an obstacle, allow it to power through instead of being deflected
        m_Rigidbody.velocity = Vector3.zero;

        // Keep the "old" velocity
        m_Rigidbody.AddForce(m_CurrentVelocity, ForceMode.VelocityChange);
    }
}
