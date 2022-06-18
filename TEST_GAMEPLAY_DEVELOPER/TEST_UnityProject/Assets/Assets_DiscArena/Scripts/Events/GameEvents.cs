using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvents", menuName = "ScriptableObjects/Events Dispatcher", order = 2)]
public class GameEvents : ScriptableObject
{
    public event UnityAction OnNewTurnEvent = null;

    public void OnNewTurn()
    {
        OnNewTurnEvent?.Invoke();
    }
}
