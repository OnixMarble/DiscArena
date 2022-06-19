using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private float m_ScalingTime = 0.0f;
    [SerializeField] private float m_ScalingSpeed = 0.2f;
    [SerializeField] private float m_TargetScale = 2.0f;
    [SerializeField] private float m_Length = 2.0f;
    private Vector3 m_Scale = Vector3.zero;

    void Update()
    {
        m_ScalingTime = Time.time * m_ScalingSpeed;
        m_Scale = new Vector3(Mathf.PingPong(m_ScalingTime, m_Length) + m_TargetScale, Mathf.PingPong(m_ScalingTime, m_Length) + m_TargetScale, 0.0f);
        transform.localScale = m_Scale;
    }
}
