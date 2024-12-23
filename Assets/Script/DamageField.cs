using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
  private BarrelController barrelController;

  private EnemyController _enemyController;
  void Start()
  {
    barrelController = FindObjectOfType<BarrelController>();
  }
  
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Player"))
    {
      if (collision.gameObject.name == "Barrel")
      {
        AttackBarrel();
      }
      else
      {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.right * 50000, transform.position);
        collision.gameObject.GetComponent<EnemyController>()?.TakeDamage(50f);
      }
    }
  }
  void AttackBarrel()
  {
    barrelController.GetDamage();
  }
}
