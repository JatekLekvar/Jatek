using UnityEngine;

public class Fire : MonoBehaviour
{
    public float direction;
    float life = 10f;

    void Update()
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime * 8f);
        life -= Time.deltaTime;

        if (life <= 0f)
        {
            Destroy(gameObject);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetHit(transform.position, 4f);
        }
    }

}
