using UnityEngine;
using System.Collections.Generic;


public class ChargingEnemy : Enemy
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
    private Vector3 _velocity;

    private int currentWaypointIndex;

    private float time = 0.0f;
    private float chargeTime = 0.0f;
    public float chargeDuration = 3f;
    public float interpolationPeriod = 3f;
    private bool nearAttack = false;
    public float hitStun = 0f;
    
    public float chargeRange = 10f;
    public float cahrgeSpeed = 20f;
    private GameObject player;

    private bool isCharging = false;

    private bool hasDirection = false;

    private Vector3 direction;

    void Awake()
    {
        _controller = GetComponent<PlayerPhysics>();
        _collider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");

        currentWaypointIndex = 0;
    }

    void Update()
    {
        //If in range, find the direction
        if(InRange(player, this.gameObject, chargeRange) && chargeTime >= chargeDuration){
            Debug.Log("Első rész");
            if(hasDirection == false){
                direction = player.transform.position - this.gameObject.transform.position;
                hasDirection = true;
            }

            //if (Waypoints[currentWaypointIndex].transform.position.x > this.transform.position.x)
            if (direction.x > this.transform.position.x)
                {
                    normalizedHorizontalSpeed = 1;
                    if (transform.localScale.x < 0f)
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                }

                else if (direction.x < this.transform.position.x)
                {
                    normalizedHorizontalSpeed = -1;
                    if (transform.localScale.x > 0f)
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                }

            chargeTime = 0f;
            isCharging = true;

            var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * cahrgeSpeed, Time.deltaTime * smoothedMovementFactor);
            _velocity.y += gravity * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);
            _velocity = _controller.velocity;
        }

        //Keep on charging
        else if(isCharging == true){
            Debug.Log("Második rész");

            var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * cahrgeSpeed, Time.deltaTime * smoothedMovementFactor);
            _velocity.y += gravity * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);
            _velocity = _controller.velocity;
        }

        //If not charging
        else{
            
            Debug.Log("Harmadik rész");

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

        }

        if(chargeTime <= chargeDuration){
            chargeTime += Time.deltaTime;
        }
        else{
            isCharging = false;
            hasDirection = false;
        }
    }

    public override void GetHit(Vector3 from, float force)
    {
        base.GetHit(from, force);

        float sign = Mathf.Sign(transform.position.x - from.x);
        _velocity.y = 12f;
        _velocity.x = sign * 20f;
        hitStun = 0.5f;
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
