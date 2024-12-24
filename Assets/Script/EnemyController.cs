using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [Header("Enemy Properties")]
  [SerializeField] private float health = 50f;
  [SerializeField] private float damageMultiplier = 1f;
  [SerializeField] private float minimumDamageThreshold = 5f;

  [Header("Effects")]
  [SerializeField] private GameObject destroyEffect;
  [SerializeField] private AudioClip hitSound;
  [SerializeField] private AudioClip destroySound;
  
  private AudioSource _audioSource;
  private SpriteRenderer _spriteRenderer;
  private static readonly WaitForSeconds DamageEffectDuration = new(0.1f);
  private Color _originalColor;
  
  private void Start()
  {
    _audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _originalColor = _spriteRenderer.color;
  }
  
  void OnCollisionEnter2D(Collision2D collision)
  {
    float impactForce = collision.relativeVelocity.magnitude;
    
    // 충격력이 최소 임계값 이상인 경우에만 데미지 계산
    if (impactForce >= minimumDamageThreshold)
    {
      TakeDamage(impactForce * damageMultiplier);
      PlayHitSound();
    }
  }
  
  public void TakeDamage(float damage)
  {
    health -= damage;

    if (health > 0)
    {
      StartCoroutine(ShowDamageEffect());
    }
    else
    {
      DestroyObstacle();
    }
  }
  
  private IEnumerator ShowDamageEffect()
  {
    _spriteRenderer.color = Color.red;
    yield return DamageEffectDuration;
    _spriteRenderer.color = _originalColor;
  }
  
  void DestroyObstacle()
  {
    if (destroyEffect)
    {
      Instantiate(destroyEffect, transform.position, Quaternion.identity);
    }

    if (destroySound)
    {
      AudioSource.PlayClipAtPoint(destroySound, transform.position);
    }

    Destroy(gameObject);
  }

  private void PlayHitSound()
  {
    if (hitSound && _audioSource)
    {
      _audioSource.PlayOneShot(hitSound);
    }
  }

  private void OnDestroy()
  {
    // 게임 매니저에 알림
    if (gameObject.CompareTag("Enemy"))
    {
      GameManager.Instance.OnEnemyDestroyed();
    }
  }
}
