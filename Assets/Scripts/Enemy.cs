using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public virtual void Punch(Vector3 from, float force)
    {
        health -= force;
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
