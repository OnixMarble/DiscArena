using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 10.0f;
    [SerializeField] private float hoverRange = 10.0f;
    [SerializeField] private float hoverSpeed = 10.0f;
    [SerializeField] private bool localSpace = false;

    void FixedUpdate()
    {
        Vector3 position = hoverHeight * hoverRange * Mathf.Cos(Time.time * hoverSpeed) * Vector3.up;

        if (localSpace)
        {
            transform.localPosition = position;
        }
        else
        {
            transform.position = position;
        }
    }
}
