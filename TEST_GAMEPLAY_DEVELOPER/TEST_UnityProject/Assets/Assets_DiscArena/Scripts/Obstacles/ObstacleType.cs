using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ObstacleType", menuName = "ScriptableObjects/Obstacle", order = 2)]
public class ObstacleType : ScriptableObject
{
    public int Health;
    public Image HealthBarImage;
}
