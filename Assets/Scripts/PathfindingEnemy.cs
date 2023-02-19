using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

enum PathfindingEnemyState
{
    Idle,
    Exploading,
    Exploaded
}
public class PathfindingEnemy : Enemy
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.25f;

    [Header("Physics")]
    public float speed = 20f;
    public float nextWaypointDistance = 1f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    public float ExplosionMaxTime = 3f;
    private float explosionTime = 0f;

    public GameObject gameController;
    private GameController gameControllerScript;
    private SpriteAnimator _animator;
    //private bool exploading;

    private Path path;
    private int currentWaypoint = 0;
    RaycastHit2D isGrounded;
    Seeker seeker;
    //Rigidbody2D rb;
    private PlayerPhysics _controller;
    private Vector3 _velocity;
    private PathfindingEnemyState state;
    private PathfindingEnemyState oldState;
    
    //private float exploadDelay = 0f;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        _controller = GetComponent<PlayerPhysics>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        gameController = GameObject.Find("Game Controller");
        gameControllerScript = gameController.GetComponent<GameController>();
        _animator = GetComponent<SpriteAnimator>();
        target = gameControllerScript.player.transform;
    }

    private void Update()
    {
        oldState = state;
        target = gameControllerScript.player.transform;

        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }

        if(InRange(this.gameObject, gameControllerScript.player,1f)){
            state = PathfindingEnemyState.Exploading;
            explosionTime += Time.deltaTime;

            if(explosionTime >= ExplosionMaxTime){
                state = PathfindingEnemyState.Exploaded;
                gameControllerScript.player.GetComponent<PlayerLogic>().GetHit(Vector3.zero,100f);
            }
        }
        else{
            explosionTime = 0;
            state = PathfindingEnemyState.Idle;
        }

        if (state != oldState)
        {
            switch (state)
            {
                case PathfindingEnemyState.Idle:
                    {
                        _animator.Play("Idle");
                    }
                    break;

                case PathfindingEnemyState.Exploading:
                    {
                        _animator.Play("Exploading");
                    }
                    break;
                case PathfindingEnemyState.Exploaded:
                    {
                        //_animator.Play("Exploaded");
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            //seeker.StartPath(rb.position, target.position, OnPathComplete);
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        //isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);
        
        // Direction Calculation
        Vector3 direction = ((Vector3)path.vectorPath[currentWaypoint] - transform.position).normalized;
        //Debug.Log("Direction: " + direction);

        if(direction.x > 0f){
            if(transform.localScale.x > 0f){
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else{
            if(transform.localScale.x < 0f){
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

        _velocity = direction * speed;

        // Jump
        /*
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }
        */

        // Movement
        //rb.AddForce(force);

        _velocity.x = Mathf.Lerp(_velocity.x,speed, Time.deltaTime);
        _controller.move(_velocity * Time.deltaTime);
        _velocity = _controller.velocity;

        // Next Waypoint
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        /*
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        */
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public bool InRange(GameObject gameObject1, GameObject gameObject2, float range)
    {
        if (Mathf.Abs(gameObject1.transform.position.x - gameObject2.transform.position.x) < range && Mathf.Abs(gameObject1.transform.position.y - gameObject2.transform.position.y) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
