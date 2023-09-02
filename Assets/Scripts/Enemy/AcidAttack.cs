using UnityEngine;

public class AcidAttack : MonoBehaviour
{
    
    [SerializeField]
    private float acidProjectileSpeed = 3.0f;

    public void Update()
    {        
        transform.Translate(acidProjectileSpeed * Time.deltaTime * Vector3.right);
    }

    private bool canDamage = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage)
        {
            Debug.Log(string.Format("{0} hits {1}", name, other.name));
            IDamageable hit = other.GetComponent<IDamageable>();
            hit?.Damage(1);
            Destroy(gameObject);
        }
    }
}
