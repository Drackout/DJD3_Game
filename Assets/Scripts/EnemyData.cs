using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData", fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public int      maxHealth;
    public float    maxIdleTime;
    public float    sightingRange;
    public float    sightingAngle;
    public float    attackRange;
    public int      attackDamage;
    public float    attackCooldown;
    public float    hurtCooldown;
}
