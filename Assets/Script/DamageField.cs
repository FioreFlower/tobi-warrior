using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log(other.name);
    if (other.tag != "Ground" && other.tag != "Player")
    {
      Destroy(other.gameObject);
    }
  }
}
