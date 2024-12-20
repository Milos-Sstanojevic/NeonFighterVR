using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    [SerializeField] private AlienSwordmasterReferences references;
    [SerializeField] private List<ParticleSystem> slashParticles;
    [SerializeField] private float timeForResetParticle = 2;
    private bool launchSlash;
    private Transform oldParent;
    private BoxCollider particleCollider;

    private void Awake()
    {
        particleCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnAlienSMSlashParticlePlay(LaunchTowardsPlayer);
    }

    private void LaunchTowardsPlayer()
    {
        particleCollider.enabled = true;
        references.IsAttacking = false;
        launchSlash = true;
        oldParent = transform.parent;
        transform.SetParent(null);

        Vector3 directionToPlayer = references.Character.transform.position - transform.position;

        directionToPlayer.y = 0;
        directionToPlayer.Normalize();

        if (directionToPlayer != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(directionToPlayer);

        transform.Rotate(0, 0, 45);
    }

    public void PlayParticles()
    {
        foreach (ParticleSystem particle in slashParticles)
            particle.Play();
    }

    public IEnumerator ResetParticles()
    {
        yield return new WaitForSeconds(timeForResetParticle);
        transform.SetParent(oldParent);
        launchSlash = false;
        transform.gameObject.SetActive(false);

        particleCollider.enabled = false;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
        if (!launchSlash) return;

        transform.position += transform.forward * Time.deltaTime * references.SlashSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
            EventManager.Instance.OnPlayerHitAction(references.AlienSMData.SpecialAttackDamage);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnAlienSMSlashParticlePlay(LaunchTowardsPlayer);
    }
}