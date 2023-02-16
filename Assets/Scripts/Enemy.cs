using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public virtual void GetHit(Vector3 from, float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        health = maxHealth;
    }
}
