using UnityEngine;

public class PunchTrigger : MonoBehaviour
{
    PlayerLogic player;
    new BoxCollider2D collider;

    private GameController gameController;

    void Start()
    {
        player = gameObject.GetComponentInParent<PlayerLogic>();
        gameController = GameObject.Find("Game Controller").gameObject.GetComponent<GameController>();
        collider = GetComponent<BoxCollider2D>();
        player = gameController.player.GetComponent<PlayerLogic>();
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
            enemy.GetHit(transform.position, player.damage);
        }
    }
}
