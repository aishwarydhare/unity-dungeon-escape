using UnityEngine;

public class AcidProjectileAnimation : MonoBehaviour
{
    private Spider spiderObj;

    void Start()
    {
        spiderObj = transform.parent.GetComponent<Spider>();
    }

    public void Fire()
    {
        Debug.Log("Spider Firing");
        spiderObj.FireAcidProjectile();
    }
}
