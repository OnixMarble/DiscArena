using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Quaternion m_TargetRotation = Quaternion.identity;
    private readonly float m_RotationSpeed = 10.0f;
    private float m_Time = 0.0f;
    private float m_Angle = 90.0f;
    private float m_OriginalAngle = 90.0f;

    public void Initialize(in float angle)
    {
        m_Angle = angle;
        m_OriginalAngle = m_Angle;
    }

    void Update()
    {
        m_Time += Time.deltaTime;

        PerformRotation();

        if (m_Time < 1.0f)
        {
            return;
        }

        m_Time = 0.0f;
        m_Angle = m_Angle == 0.0f ? m_Angle = m_OriginalAngle : m_Angle = 0.0f;
    }

    private void PerformRotation()
    {
        m_TargetRotation = Quaternion.AngleAxis(m_Angle, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetRotation, m_RotationSpeed * Time.deltaTime);
    }
}
