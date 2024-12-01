using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableGun", menuName = "NeonFighterVR/Scriptable Gun")]
public class GunData : ScriptableObject
{
    public int GunDamage;
    public float FireRate;
    public float WeaponRange;
    public float BigBulletWeaponRange;
    public float HitForce;
    public int Ammo;
    public int BulletSpeed;
    public int BigBulletSpeed;
}
