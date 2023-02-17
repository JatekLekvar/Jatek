using UnityEngine;

public class ChargingEnemy : Enemy
{
    public GameObject firePrefab;
    private float gravity = -64f;
    private float tmpRunSpeed;
    public float runSpeed = 10f;
    private float groundDamping = 28f;
    private float inAirDamping = 8f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 1;

    private PlayerPhysics _controller;
    private BoxCollider2D _collider;
    private Vector3 _velocity;

    private float chargeTime = 0.0f;
    public float chargeDuration = 3f;
    public float interpolationPeriod = 3f;
    public float hitStun = 0f;
    
    public float chargeRange = 10f;
    public float cahrgeSpeed = 20f;
    private GameObject player;

    private bool isCharging = false;
    private bool hasDirection = false;

    private Vector3 direction;

    public float movementMaxTime;
    private float movementTimer;

    public float restMaxTime;
    private float restingTime;
    public float stopMaxTime;
    private float stopTime;

    private bool shouldFlip = false;
    void Awake()
    {
        _controller = GetComponent<PlayerPhysics>();
        _collider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");

        tmpRunSpeed = runSpeed;
        stopTime = stopMaxTime;
        restingTime = restMaxTime;
        chargeTime = chargeDuration;
    }

    void Update()
    {
        //If in range, find the direction
        if(InRange(player, this.gameObject, chargeRange) && chargeTime >= chargeDuration && stopTime >= stopMaxTime){
            //Debug.Log("Első rész");
            if(hasDirection == false){
                direction = player.transform.position - this.gameObject.transform.position;
                hasDirection = true;
            }

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
        else if(isCharging == true && stopTime >= stopMaxTime){
           //Debug.Log("Második rész");

            var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * cahrgeSpeed, Time.deltaTime * smoothedMovementFactor);
            _velocity.y += gravity * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);
            _velocity = _controller.velocity;
        }

        //If not charging
        else if(stopTime >= stopMaxTime){
            
            //Debug.Log("Harmadik rész");

            if (movementTimer >= movementMaxTime)
            {
                //normalizedHorizontalSpeed = normalizedHorizontalSpeed * -1;
                movementTimer = 0f;
                restingTime = 0f;
            }

            if (hitStun > 0f)
            {
                hitStun -= Time.deltaTime;
                normalizedHorizontalSpeed = 0f;
            }
            else
            {
                transform.localScale = new Vector3(normalizedHorizontalSpeed, transform.localScale.y, transform.localScale.z);
            }

            var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
            _velocity.y += gravity * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);
            _velocity = _controller.velocity;

        }

        //Increase time until chargeDuration is reached
        if(chargeTime <= chargeDuration){
            chargeTime += Time.deltaTime;
        }
        //When done charging
        else if (isCharging){
            isCharging = false;
            hasDirection = false;
            stopTime = 0f;
            //Debug.Log("I'm stopping");
        }

        if(stopTime <= stopMaxTime){
            stopTime += Time.deltaTime;
            //Debug.Log("Being stopped");
        }
        else{
            if(restingTime <= restMaxTime){
                restingTime += Time.deltaTime;
                movementTimer = 0f;
                runSpeed = 0f;
                shouldFlip = true;
                //Debug.Log("Resting...");
            }
            else if (shouldFlip){
                if(!isCharging){
                    normalizedHorizontalSpeed = normalizedHorizontalSpeed * -1;
                }
                shouldFlip = false;
            }
            else if(movementTimer <= movementMaxTime){
                runSpeed = tmpRunSpeed;
                movementTimer += Time.deltaTime;
            }
            
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
