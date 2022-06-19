using System;
using UnityEngine;

public class DiscProjectile : MonoBehaviour
{
    [SerializeField] private DiscData m_DiscType = null;
    public event Action<float, int> OnCollisionEvent = null;
    private Rigidbody m_Rigidbody = null;
    private Renderer m_Renderer = null;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Renderer = GetComponentInChildren<Renderer>();
    }

    public void SetDiscData(in DiscData discData)
    {
        m_DiscType = discData;
        m_Renderer.material = m_DiscType.DiscMaterial;
    }

    public void ShootDisc(in Vector2 touchPosition)
    {
        Vector3 targetPosition = GetTouchWorldPosition(touchPosition);
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.AddForce(direction * m_DiscType.Speed, ForceMode.Impulse);
    }

    public void ClampSpeed()
    {
        float maxSpeed = m_DiscType.Speed;
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, maxSpeed);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            OnCollisionEvent?.Invoke(m_DiscType.Damage, collision.gameObject.GetInstanceID());
        }
    }
}
