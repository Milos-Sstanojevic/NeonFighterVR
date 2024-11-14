using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableGun", menuName = "NeonFighterVR/Scriptable Gun")]
public class GunData : ScriptableObject
{
    public int GunDamage;
    public float FireRate;
    public float WeaponRange;
    public float HitForce;
    public int Ammo;
}
