using UnityEngine;

public class PunchTrigger : MonoBehaviour
{
    PlayerLogic player;
    new BoxCollider2D collider;

    void Start()
    {
        player = gameObject.GetComponentInParent<PlayerLogic>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        collider.enabled = player.isPunching;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetHit(transform.position, 100f);
        }
    }
}
