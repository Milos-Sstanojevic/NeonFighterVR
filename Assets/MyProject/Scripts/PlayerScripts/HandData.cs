using UnityEngine;

public class HandData : MonoBehaviour
{
	public enum HandModelType { Left, Right }

	public HandModelType handModelType;
	public Transform Root;
	public Animator Animator;
	public Transform[] FingerBones;
	public GameObject WeaponInHand;
	public bool HasWeaponInHand;
	public Transform GunSpinningPivot;

}
