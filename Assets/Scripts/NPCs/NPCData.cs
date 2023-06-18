using UnityEngine;

[CreateAssetMenu(menuName = "NPCData", fileName = "NPCData")]
public class NPCData : ScriptableObject
{
    public int      maxHealth;
    public float    maxIdleTime;
    public float    sightingRange;
    public float    sightingAngle;
    public float    hurtCooldown;
    public int      scorePoints;
}
