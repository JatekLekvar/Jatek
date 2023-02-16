using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class ChargingEnemy : Enemy
{
    public List<GameObject> Waypoints = new List<GameObject>();
    private int currentWaypointIndex;

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
    public float chargeSpeed = 40f;

    public float gravity = -64f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    public GameObject gameController;

    private Path path;
    private int currentWaypoint = 0;
    RaycastHit2D isGrounded;
    Seeker seeker;
    //Rigidbody2D rb;
    private PlayerPhysics _controller;
    private Vector3 _velocity;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        _controller = GetComponent<PlayerPhysics>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        gameController = GameObject.Find("Game Controller");
    }

    private void FixedUpdate()
    {
        target = gameController.GetComponent<GameController>().player.transform;

        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
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
        _velocity.y += gravity * Time.deltaTime;
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
}
