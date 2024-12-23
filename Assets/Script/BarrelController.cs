using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    public GameObject boom;
    public GameObject barrel;
    
    bool isBoom = false;
    public bool isDone = false;
    public void GetDamage()
    {
        boom.SetActive(true);
        barrel.SetActive(false);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        isBoom = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;
        if (impactForce >= 1f)
        {
            GetDamage();
        }
    }

    IEnumerator DestroyBoom()
    {
        if (isBoom)
        {
            yield return new WaitForSeconds(1f);
            boom.SetActive(false);
            isDone = true;
        }
    }

    void Update()
    {
        StartCoroutine(DestroyBoom());
    }
}
