using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDamageField : MonoBehaviour
{
    public Rigidbody2D rb;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ground") && !other.CompareTag("Player"))
        {
          rb.AddForceAtPosition( Vector2.one*10000f, transform.position, ForceMode2D.Impulse);
          Destroy(gameObject);
        }
    }
}
