using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float defaultHP = 100f;
    private float HP;
    private Animator animator;
    private ParticleSystem DeathEffect;
    private ParticleSystemRenderer DeathFXRenderer;
    private SpriteRenderer playerSprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        DeathEffect = gameObject.transform.Find("DeathEffect").GetComponent<ParticleSystem>();
        DeathFXRenderer = gameObject.transform.Find("DeathEffect").GetComponent<ParticleSystemRenderer>();
        ResetHealth();
    }

    private void Update()
    {
        animator.SetFloat("Health", HP);
    }

    public void SetDefaultHP(float hp) { 
        defaultHP = hp;
    }

    public void Heal(float hp)
    {
        HP += hp;
    }

    public void TakeDamage(float hp)
    {
        if (GameManager.Instance.CompareStatus(GameStatus.IMMORTAL)) return;

        HP -= hp;
        animator.SetTrigger("DoDamage");

        if (HP <= 0f)
        {
            HP = 0f;
            StartCoroutine("HandleDeathAnimation");
        }
    }

    public void ResetHealth()
    {
        HP = defaultHP;
    }

    public float GetHealth()
    {
        return HP;
    }

    public void KillPlayer()
    {
        TakeDamage(HP);
    }

    IEnumerator HandleDeathAnimation()
    {
        float rotationDirection = Random.Range(-6f, 6f);

        var rotationModule = DeathEffect.rotationBySpeed;
        rotationModule.z = rotationDirection;

        var renderModuleFlip = DeathFXRenderer.flip;
        renderModuleFlip.x = playerSprite.flipX ? 1f : 0f;

        GameManager.Instance.ChangeStatus(GameStatus.DEATH);
        GameManager.Instance.CameraStopFollow();
        GameManager.Instance.TakeLive();

        playerSprite.enabled = false;

        DeathEffect.Play();
        print(GameManager.Instance.GetLives());

        if (GameManager.Instance.GetLives() < 1)
            AudioController.Instance.StopBGM();

        yield return new WaitForSeconds(1f);

        if (GameManager.Instance.GetLives() < 1)
        {
            GameManager.Instance.PlayerDeath();
            StopCoroutine("HandleDeathAnimaion");
        }
        else
        {
            GameManager.Instance.RespawnPlayer();
        }
    }
}
