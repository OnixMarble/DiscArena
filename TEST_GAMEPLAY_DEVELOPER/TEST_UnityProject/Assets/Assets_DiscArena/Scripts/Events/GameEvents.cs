using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvents", menuName = "ScriptableObjects/Events Dispatcher", order = 2)]
public class GameEvents : ScriptableObject
{
    public event UnityAction OnNewTurnEvent = null;
    public event UnityAction OnObstacleDestroyedEvent = null;
    public event UnityAction<bool> OnGameEndedEvent = null;
    public event UnityAction<int> OnUpdateTotalDiscsEvent = null;
    public event UnityAction<float, int> OnCollisionEvent = null;

    public enum DiscTypes { Classic, Sprinter };
    public event UnityAction<DiscTypes> OnSwapDiscsEvent = null;

    public void OnNewTurn() => OnNewTurnEvent?.Invoke();

    public void OnEndGame(bool winner) => OnGameEndedEvent?.Invoke(winner);
    
    public void OnObstacleDestroyed() => OnObstacleDestroyedEvent?.Invoke();

    public void OnCollision(float damage, int obstacleID) => OnCollisionEvent?.Invoke(damage, obstacleID);

    public void OnSwapDiscs(DiscTypes newDiscType) => OnSwapDiscsEvent?.Invoke(newDiscType);

    public void OnUpdateDiscsCount(int totalDiscs) => OnUpdateTotalDiscsEvent?.Invoke(totalDiscs);
}
