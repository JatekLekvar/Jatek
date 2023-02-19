using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

enum State
{
    Idle,
    Walk,
    Jump,
    Crouch,
    Attack,
}


public class PlayerLogic : MonoBehaviour
{
    public List<string> heldAbilities = new List<string>();
    public GameObject firePrefab;
    public GameObject gameController;
    public GameController gameControllerScript;
    public bool isPunching;
    public int upgrades = 0;

    private float gravity = -64f;
    private float runSpeed = 10f;
    private float groundDamping = 28f;
    private float inAirDamping = 8f;
    private float jumpHeight = 12f;
    private float jumpLength = 0.2f;
    private float attackLength = 0.5f;

    public float maxHealth = 300f;
    public float currentHealth;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private PlayerPhysics _controller;
    private RaycastHit2D _lastControllerColliderHit;
    private SpriteAnimator _animator;
    private BoxCollider2D _collider;
    private Vector3 _velocity;
    private float _jumpTimer;
    private float _attackTimer;
    private float _attackCooldown;
    private State state = State.Idle;
    private bool left;

    public float invincibleMaxTime;
    public float invincibleCurrentTimer = 0f;

    public float Gravity { get => gravity; set => gravity = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float GroundDamping { get => groundDamping; set => groundDamping = value; }
    public float InAirDamping { get => inAirDamping; set => inAirDamping = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float JumpLength { get => jumpLength; set => jumpLength = value; }
    public float AttackLength { get => attackLength; set => attackLength = value; }

    void Awake()
    {
        _controller = GetComponent<PlayerPhysics>();

        DontDestroyOnLoad(gameObject);

        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        // _controller.onTriggerExitEvent += onTriggerExitEvent;

        _animator = GetComponent<SpriteAnimator>();
        _collider = GetComponent<BoxCollider2D>();
        gameController = GameObject.Find("Game Controller");
        gameControllerScript = gameController.GetComponent<GameController>();
        gameControllerScript.player = this.gameObject;
        

        currentHealth = maxHealth;
    }

    void Update()
    {
        bool leftBefore = left;
        State oldState = state;
        state = State.Idle;

        if (invincibleMaxTime - invincibleCurrentTimer > 0f)
        {
            invincibleCurrentTimer += Time.deltaTime;
        }

        bool crouch = Input.GetKey(KeyCode.S) && _controller.isGrounded;

        if (Input.GetKey(KeyCode.D))
        {
            if (!crouch)
            {
                state = State.Walk;
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            left = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!crouch)
            {
                state = State.Walk;
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            left = true;
        }
        else
        {
            normalizedHorizontalSpeed = 0;
        }

        if (crouch)
        {
            normalizedHorizontalSpeed = 0;

        }


        if (_controller.isGrounded && Input.GetKey(KeyCode.W))
        {
            if(gameControllerScript.spaceShipEntrance != null && InRange(this.gameObject,gameControllerScript.spaceShipEntrance,4f)){
                gameControllerScript.nextWorldEnterSide = NextWorldEnterSide.Up;
                SceneManager.LoadScene("Space Ship");
                return;
            }
            _jumpTimer = JumpLength;
        }

        if (_jumpTimer > 0f && Input.GetKey(KeyCode.W))
        {
            _jumpTimer -= Time.deltaTime;
            _velocity.y = JumpHeight;
        }

        if (!Input.GetKey(KeyCode.W))
        {
            _jumpTimer = 0f;
        }

        if (!_controller.isGrounded)
        {
            state = State.Jump;
        }

        bool spit = false;
        if(upgrades >= 2){
            spit = Input.GetKey(KeyCode.LeftShift);
        }

        if (crouch)
        {
            // state = State.Crouch;

            // _collider.offset = new Vector2(0f, -1f);
            // _collider.size = new Vector2(2f, 1f);
        }
        else
        {
            if (state != State.Attack && _attackCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
            {
                state = State.Attack;
                _attackTimer = AttackLength;

                if (spit && upgrades >= 2)
                {
                    Vector3 spawnPos = new Vector3 (transform.position.x ,transform.position.y-1 ,transform.position.z);
                    GameObject fire = (GameObject)Instantiate(firePrefab, spawnPos, Quaternion.identity);
                    Fire fireComp = fire.GetComponent<Fire>();
                    fireComp.direction = left ? -1f : 1f;
                }
            }

            _collider.offset = new Vector2(0f, -0.78f);
            _collider.size = new Vector2(2f, 2.5f);
        }

        if (_attackTimer > 0f)
        {
            state = State.Attack;
            _attackTimer -= Time.deltaTime;
            _attackCooldown = 0.1f;

            if (_controller.isGrounded)
            {
                normalizedHorizontalSpeed = 0f;
            }
        }

        if (state != State.Attack && _attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }

        var smoothedMovementFactor = _controller.isGrounded ? GroundDamping : InAirDamping;
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * RunSpeed, Time.deltaTime * smoothedMovementFactor);
        _velocity.y += Gravity * Time.deltaTime;


        if (leftBefore != left)
        {
            _velocity.y += 6f;
            state = State.Attack;
        }


        _controller.move(_velocity * Time.deltaTime);
        _velocity = _controller.velocity;

        if (state != oldState)
        {
            switch (state)
            {
                case State.Idle:
                    {
                        isPunching = false;
                        switch(upgrades){
                            case 0 : _animator.Play("Idle");
                            break;
                            case 1 : _animator.Play("Idle1");
                            break;
                            case 2 : _animator.Play("Idle2");
                            break;
                            case 3 : _animator.Play("Idle3");
                            break;
                        }
                        //_animator.Play("Idle");
                    }
                    break;

                case State.Walk:
                    {
                        isPunching = false;
                        //_animator.Play("Walk");
                        switch(upgrades){
                            case 0 : _animator.Play("Walk");
                            break;
                            case 1 : _animator.Play("Walk1");
                            break;
                            case 2 : _animator.Play("Walk2");
                            break;
                            case 3 : _animator.Play("Walk3");
                            break;
                        }
                    }
                    break;

                case State.Jump:
                    {
                        isPunching = false;
                        //_animator.Play("Jump");
                        switch(upgrades){
                            case 0 : _animator.Play("Jump");
                            break;
                            case 1 : _animator.Play("Jump1");
                            break;
                            case 2 : _animator.Play("Jump2");
                            break;
                            case 3 : _animator.Play("Jump3");
                            break;
                        }
                    }
                    break;

                case State.Crouch:
                    {
                        isPunching = false;
                        _animator.Play("Crouch");
                    }
                    break;

                case State.Attack:
                    {
                        if (spit && upgrades >= 2)
                        {
                            isPunching = false;
                            switch(upgrades){
                            case 2 : _animator.Play("Spit2");
                            break;
                            case 3 : _animator.Play("Spit3");
                            break;
                            }
                            
                            //_animator.Play("Spit");
                            
                        }
                        else
                        {
                            isPunching = true;
                            //_animator.Play("Punch");
                            switch(upgrades){
                            case 0 : _animator.Play("Punch");
                            break;
                            case 1 : _animator.Play("Punch1");
                            break;
                            case 2 : _animator.Play("Punch2");
                            break;
                            case 3 : _animator.Play("Punch3");
                            break;
                        }
                        }
                    }
                    break;
            }
        }
    }

    public virtual void GetHit(Vector3 from, float damageAmount)
    {
        if (invincibleCurrentTimer <= invincibleMaxTime)
        {
            return;
        }

        float sign = Mathf.Sign(transform.position.x - from.x);
        _velocity.y = 12f;
        _velocity.x = sign * 12f;

        invincibleCurrentTimer = 0f;
        currentHealth -= damageAmount;
        if (currentHealth <= 0f)
        {
            PlayerDeath();
        }
    }

    void onControllerCollider(RaycastHit2D hit)
    {
        if (hit.normal.y < 0f && _velocity.y > 0f)
        {
            _velocity = new Vector3(_velocity.x, 0f, _velocity.z);
            _jumpTimer = 0f;
        }
    }

    void onTriggerEnterEvent(Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.name == "AbilityTrigger")
        {
            obj = obj.transform.parent.gameObject;
            Ability ability = obj.GetComponent<Ability>();
            if(ability.identifier == "upgrade1"){
                upgrades = 1;
            }
            else if(ability.identifier == "upgrade2"){
                upgrades = 2;
            }
            else if(ability.identifier == "upgrade3"){
                upgrades = 3;
            }
            else{
                gameController.GetComponent<Inventory>().AddToInvertory(obj);
                heldAbilities.Add(ability.identifier);
            }
            //Destroy(obj);
            obj.SetActive(false);
            //obj.transform.position.Set(-100,0,0);
        }
    }

    void PlayerDeath()
    {
        gameController.GetComponent<GameController>().RefreshPlayer();
        Destroy(this.gameObject);
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
