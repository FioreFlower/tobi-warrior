using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [Header("Enemy Properties")]
  public float health = 5f;
  public float damageMultiplier = 1f;
  public float minimumDamageThreshold = 5f;

  [Header("Effects")]
  public GameObject destroyEffect;
  public AudioClip hitSound;
  public AudioClip destroySound;

  private AudioSource audioSource;
  
  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
      audioSource = gameObject.AddComponent<AudioSource>();
    }
  }
  
  void OnCollisionEnter2D(Collision2D collision)
  {
    float impactForce = collision.relativeVelocity.magnitude;

    // 최소 충격량보다 작으면 데미지 없음
    if (impactForce < minimumDamageThreshold) return;

    // 데미지 계산
    float damage = impactForce * damageMultiplier;
    TakeDamage(damage);

    // 충돌 사운드
    PlayHitSound();
  }
  
  void TakeDamage(float damage)
  {
    health -= damage;

    // 데미지 시각 효과
    StartCoroutine(DamageEffect());

    if (health <= 0)
    {
      DestroyObstacle();
    }
  }
  
  IEnumerator DamageEffect()
  {
    // 색상 변경으로 데미지 표시
    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
    Color originalColor = sprite.color;
    sprite.color = Color.red;

    yield return new WaitForSeconds(0.1f);

    sprite.color = originalColor;
  }
  
  void DestroyObstacle()
  {
    GameObject effect = null;
    // 파괴 효과 생성
    if (destroyEffect != null)
    {
      effect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
    }

    // 파괴 사운드 재생
    if (destroySound != null)
    {
      AudioSource.PlayClipAtPoint(destroySound, transform.position);
    }

    // 게임 매니저에 알림
    GameManager.Instance.OnEnemyDestroyed();

    Destroy(gameObject);
    if (effect !=null ) Destroy(effect);
  }

  void PlayHitSound()
  {
    if (hitSound != null && audioSource != null)
    {
      audioSource.PlayOneShot(hitSound);
    }
  }
  
}
