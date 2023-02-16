using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public float horizontalDirection;
    public float verticalDirection;
    float life = 5f;


    void Update()
    {
        transform.Translate(new Vector3(horizontalDirection, verticalDirection, 0f) * Time.deltaTime * 8f);
        life -= Time.deltaTime;

        if (life <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerLogic playerLogic = collider.gameObject.GetComponent<PlayerLogic>();
        if (playerLogic != null)
        {
            playerLogic.GetHit(transform.position, 100f);
            Destroy(gameObject);
        }
    }

}
