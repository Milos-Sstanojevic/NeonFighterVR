using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private int maxHits = 4;
    [SerializeField] private float timeToAttackShield = 1;
    [SerializeField] private float ripplesVisibleTime = 2;
    [SerializeField] private GameObject shieldRipples;
    [SerializeField] private int hitCount;
    private VisualEffect shieldVFX;
    private Color originalColor;
    private Coroutine decreaseCoroutine;
    private AlienGunslingerController alienGunslingerController;
    private bool bulletSteered;

    private void Awake()
    {
        alienGunslingerController = GetComponentInParent<AlienGunslingerController>();
        shieldVFX = GetComponent<VisualEffect>();
        originalColor = shieldVFX.GetVector4("FresnelColor");
    }

    private void OnEnable()
    {
        hitCount = 0;
        shieldVFX.SetVector4("FresnelColor", originalColor);
    }

    public void ActivateRipples()
    {
        if (bulletSteered)
        {
            bulletSteered = false;
            return;
        }

        if (alienGunslingerController.GetCurrentState() == typeof(AGS_State_IdleProvoking))
        {
            EventManager.Instance.OnShieldHitAction();
            return;
        }

        if (alienGunslingerController.GetCurrentState() == typeof(AGS_State_PunishPlayer))
            return;

        hitCount++;

        if (hitCount >= maxHits)
        {
            BreakShield();
            return;
        }

        if (shieldRipples.activeSelf)
        {
            StopCoroutine(DisableRipples());
            shieldRipples.SetActive(false);
        }

        shieldRipples.SetActive(true);
        StartCoroutine(DisableRipples());

        SetFresnelColor();

        if (decreaseCoroutine != null)
            StopCoroutine(decreaseCoroutine);

        decreaseCoroutine = StartCoroutine(DecreaseFresnelColor());
    }

    private void BreakShield()
    {
        StopCoroutine(decreaseCoroutine);
        gameObject.SetActive(false);
        Debug.Log("SHIELD BROKEN");
        EventManager.Instance.OnShieldBrokenAction();
        hitCount = 0;
        shieldVFX.SetVector4("FresnelColor", originalColor);
    }

    public IEnumerator DecreaseFresnelColor()
    {
        while (hitCount > 0)
        {
            yield return new WaitForSeconds(timeToAttackShield);
            hitCount--;
            SetFresnelColor();
        }
    }

    private void SetFresnelColor()
    {
        shieldVFX.SetVector4("FresnelColor", originalColor * Mathf.Pow(2, Mathf.Pow(3, hitCount)));
    }

    private IEnumerator DisableRipples()
    {
        yield return new WaitForSeconds(ripplesVisibleTime);
        shieldRipples.SetActive(false);
    }

    public void RecoverShield()
    {
        gameObject.SetActive(true);
        hitCount = 0;
        shieldRipples.SetActive(false);
        alienGunslingerController.ShieldRecovered();
        EventManager.Instance.OnShieldRecoveredAction();
    }

    public void BulletSteered() => bulletSteered = true;
}
