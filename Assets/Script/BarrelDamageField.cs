using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDamageField : MonoBehaviour
{
  [Header("Explosion Settings")]
  public float explosionForce = 1f; // 폭발력
  public float explosionRadius = 5f; // 폭발 반경
  [Header("References")]
  public Rigidbody2D rb;
  
  private void OnCollisionEnter2D(Collision2D collision)
  {
    // 특정 태그는 무시
    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player")) return;

    // 폭발 반경 내 객체에 폭발력 적용
    Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    foreach (Collider2D hit in hitObjects)
    {
      ApplyExplosionForce(hit);
    }
  }

  private void ApplyExplosionForce(Collider2D target)
  {
    Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
    if (targetRb != null && targetRb != rb)
    {
      // 폭발 중심에서 대상 방향 계산
      Vector2 direction = (target.transform.position - transform.position).normalized;
      // 플레이어인지 확인하고 폭발력 조정
      float adjustedForce = target.CompareTag("Player") ? explosionForce * 0.01f : explosionForce;
      targetRb.AddForce(direction * adjustedForce, ForceMode2D.Impulse);
    }
  }
}
