using UnityEngine;

public class DiscProjectile : MonoBehaviour
{
    [SerializeField] private float m_ForceStrength = 25.0f;
    private Rigidbody m_Rigidbody = null;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void ShootDisc(in Vector2 touchPosition)
    {
        Vector3 targetPosition = GetTouchWorldPosition(touchPosition);
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.AddForce(direction * m_ForceStrength, ForceMode.Impulse);
    }

    private Vector3 GetTouchWorldPosition(in Vector2 touchPosition)
    {
        float maximumRayDistance = 300.0f;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, maximumRayDistance))
        {
            // Return the ray's impact point in world space coordinate
            return hitData.point;
        }

        return Vector3.zero;
    }
}
