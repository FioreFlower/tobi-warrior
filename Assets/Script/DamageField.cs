using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
  private BarrelController barrelController;

  void Start()
  {
    barrelController = FindObjectOfType<BarrelController>();
  }
  
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Player"))
    {
      if (collision.gameObject.name == "Barrel") AttackBarrel();
      else
      {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.right*500, transform.position);
      }
    }
  }
  void AttackBarrel()
  {
    barrelController.GetDamage();
  }
}
