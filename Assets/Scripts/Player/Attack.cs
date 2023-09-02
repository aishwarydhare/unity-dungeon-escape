using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private bool canDamage = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage)
        {
            Debug.Log(string.Format("Hit: {0}", other.name));
            IDamageable hit = other.GetComponent<IDamageable>();
            hit?.Damage(1);
            canDamage = false;
            StartCoroutine(CanDamageCooldown());
        }
    }

    IEnumerator CanDamageCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canDamage = true;
        StopCoroutine(CanDamageCooldown());
    }
}
