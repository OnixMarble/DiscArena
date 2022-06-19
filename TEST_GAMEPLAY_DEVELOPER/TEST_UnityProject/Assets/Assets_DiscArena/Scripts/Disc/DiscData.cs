using UnityEngine;

[CreateAssetMenu(fileName = "Disc", menuName = "ScriptableObjects/Disc", order = 3)]
public class DiscData : ScriptableObject
{
    public int Speed;
    public float Damage;
    public Material DiscMaterial;
}
