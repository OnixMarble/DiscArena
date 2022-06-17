using UnityEngine;

public class Disc : MonoBehaviour
{
    [SerializeField] private InputReader m_InputReader = null;
    [SerializeField] private float m_ForceStrength = 25.0f;
    private Rigidbody m_Rigidbody = null;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SetupCallbacks(true);
    }

    private void OnDisable()
    {
        SetupCallbacks(false);
    }

    private void SetupCallbacks(bool bind)
    {
        if (bind)
        {
            m_InputReader.OnShootEvent += ShootDisc;
        }
        else
        {
            m_InputReader.OnShootEvent -= ShootDisc;
        }
    }

    public void ShootDisc(Vector2 mousePosition)
    {
        Vector3 targetPosition = Vector3.zero;

        float maximumDistance = 500.0f;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hitData, maximumDistance))
        {
            targetPosition = hitData.point;
        }

        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.AddForce(direction * m_ForceStrength, ForceMode.Impulse);
    }
}
