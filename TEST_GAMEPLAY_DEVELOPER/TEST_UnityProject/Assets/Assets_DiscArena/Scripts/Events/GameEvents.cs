using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvents", menuName = "ScriptableObjects/Events Dispatcher", order = 2)]
public class GameEvents : ScriptableObject
{
    public event UnityAction OnNewTurnEvent = null;
    public event UnityAction OnObstacleDestroyedEvent = null;
    public event UnityAction<float, int> OnCollisionEvent = null;

    public void OnNewTurn() => OnNewTurnEvent?.Invoke();

    public void OnObstacleDestroyed() => OnObstacleDestroyedEvent?.Invoke();

    public void OnCollision(float damage, int obstacleID) => OnCollisionEvent?.Invoke(damage, obstacleID);
}
