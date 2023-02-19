using UnityEngine;
using System.Collections.Generic;


public class BasicEnemy : Enemy
{

    public List<GameObject> Waypoints = new List<GameObject>();
    public GameObject firePrefab;

    private float gravity = -64f;
    public float runSpeed = 10f;
    private float groundDamping = 28f;
    private float inAirDamping = 8f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private PlayerPhysics _controller;
    private BoxCollider2D _collider;
    private SpriteAnimator _animator;
    private Vector3 _velocity;

    private int currentWaypointIndex;

    private float time = 0.0f;
    public float interpolationPeriod = 3f;
    private bool nearAttack = false;
    float hitStun = 0f;

    void Awake()
    {
        _controller = GetComponent<PlayerPhysics>();
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<SpriteAnimator>();

        currentWaypointIndex = 0;
    }

    void Update()
    {
        if (Mathf.Abs(this.transform.position.x - Waypoints[currentWaypointIndex].transform.position.x) < 0.3f)
        {

            if (currentWaypointIndex == 0)
            {
                currentWaypointIndex = 1;
            }
            else
            {
                currentWaypointIndex = 0;
            }
        }

        if (hitStun > 0f)
        {
            hitStun -= Time.deltaTime;
            normalizedHorizontalSpeed = 0f;
        }
        else
        {
            if (Waypoints[currentWaypointIndex].transform.position.x > this.transform.position.x)
            {
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }

            else if (Waypoints[currentWaypointIndex].transform.position.x < this.transform.position.x)
            {
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
        }

        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
        _velocity.y += gravity * Time.deltaTime;

        if (Mathf.Abs(time - interpolationPeriod) < 0.4f && !nearAttack)
        {
            nearAttack = true;
        }
        else
        {
            nearAttack = false;
        }

        if (nearAttack)
        {
            _velocity = Vector3.zero;
        }

        _controller.move(_velocity * Time.deltaTime);
        _velocity = _controller.velocity;

        time += Time.deltaTime;

        //Attack
        if (time >= interpolationPeriod)
        {
            time = 0.0f;

            GameObject fire = (GameObject)Instantiate(firePrefab, transform.position, Quaternion.identity);
            EnemyFire fireComp = fire.GetComponent<EnemyFire>();
            fireComp.horizontalDirection = -1f;
            fireComp.verticalDirection = 0f;

            fire = (GameObject)Instantiate(firePrefab, transform.position, Quaternion.identity);
            fireComp = fire.GetComponent<EnemyFire>();
            fireComp.horizontalDirection = 1f;
            fireComp.verticalDirection = 0f;

            fire = (GameObject)Instantiate(firePrefab, transform.position, Quaternion.identity);
            fireComp = fire.GetComponent<EnemyFire>();
            fireComp.horizontalDirection = 0f;
            fireComp.verticalDirection = 1f;
        }

    }

    public override void GetHit(Vector3 from, float force)
    {
        base.GetHit(from, force);
        //Debug.Log("Enemy megütve");
        float sign = Mathf.Sign(transform.position.x - from.x);
        _velocity.y = 12f;
        _velocity.x = sign * 20f;
        hitStun = 0.5f;
        health -= force;
        /*
        if(health <= 0){
            Destroy(this.gameObject);
        }
        */
    }
}
