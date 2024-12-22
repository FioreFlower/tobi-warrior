using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log(other.name);
    if (!other.CompareTag("Ground") && !other.CompareTag("Player"))
    {
      Destroy(other.gameObject);
    }
  }
}
