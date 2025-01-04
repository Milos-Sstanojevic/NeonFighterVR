using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "NeonFighterVR/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    public int Health;
    public int Damage;
    public int SpecialAttackDamage;
    public float SecondPhaseHPRatio;
}