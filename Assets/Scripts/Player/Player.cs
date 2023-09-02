using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int Health { get; set; }

    [SerializeField]
    protected int health;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private LayerMask groundLayer;

    private GameObject swordArc;
    private Animator swordAnimator;
    private SpriteRenderer swordSpriteRenderer;

    private GameObject playerObject;
    private SpriteRenderer playerSpriteRenderer;
    private Animator playerAnimator;
    private Rigidbody2D _rigidbody2D;
    private bool flipped = false;

    private void Init()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        playerObject = transform.GetChild(0).gameObject;
        playerAnimator = playerObject.GetComponent<Animator>();
        playerSpriteRenderer = playerObject.GetComponent<SpriteRenderer>();

        swordArc = transform.GetChild(1).gameObject;
        swordAnimator = swordArc.GetComponent<Animator>();
        swordSpriteRenderer = swordArc.GetComponentInChildren<SpriteRenderer>();

        Health = health;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        Movement();
        JumpCheck();
        FireSwingCheck();
    }

    private void Movement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move > 0) flipped = false;
        else if (move < 0) flipped = true;
        playerSpriteRenderer.flipX = flipped;

        bool walking = move > 0.1f || move < -0.1f;
        playerAnimator.SetBool("Walk", walking);

        float playerSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimator.SetBool("Run", true);
            playerSpeed *= 2;
        }
        else
        {
            playerAnimator.SetBool("Run", false);
        }

        _rigidbody2D.velocity = new Vector2(move * playerSpeed, _rigidbody2D.velocity.y);
    }

    private void JumpCheck()
    {
        bool isPlayerGrounded = IsPlayerGrounded();

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isPlayerGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        playerAnimator.SetBool("Jump", !isPlayerGrounded);
    }

    private bool IsPlayerGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            .8f,
            groundLayer
        );
        Debug.DrawRay(transform.position, Vector2.down * .7f, Color.green);

        if (hit.collider != null)
        {
            // Debug.Log("hit " + hit.collider.name);
            return true;
        }

        return false;
    }

    private void FireSwingCheck()
    {
        if (Input.GetMouseButtonDown(0) && IsPlayerGrounded())
        {
            // Debug.Log("mouse 0 down");
            playerAnimator.SetTrigger("Fire_Swing");

            swordSpriteRenderer.flipX = flipped;
            swordSpriteRenderer.flipY = flipped;
            swordArc.transform.localPosition = new Vector3()
            {
                x = 1.01f * (flipped ? -1 : 1),
                y = -0.09f,
                z = -0.25f
            };
            swordAnimator.SetTrigger(flipped ? "Sword_Animation_Reverse" : "Sword_Animation");
        }
    }

    public void Damage(int attackPower)
    {
        Health -= attackPower;
        if (Health <= 0)
        {
            Debug.Log("gameover");
        }
    }
}
