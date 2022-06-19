using UnityEngine;

public enum ObstacleType { Regular, Chest }
public enum ObstacleBehaviour { None, RotateRight, RotateLeft }

[CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/Obstacle", order = 4)]
public class ObstacleTypes : ScriptableObject
{
    public float Health;
    public ObstacleType Type;
    public ObstacleBehaviour Behaviour;
}
