using UnityEngine;

public enum ObstacleType
{
    Regular,
    Chest
}

[CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/Obstacle", order = 4)]
public class ObstacleTypes : ScriptableObject
{
    public float Health;
    public ObstacleType Type;
}
