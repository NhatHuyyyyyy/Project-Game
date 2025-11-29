using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } }
    public static PlayerController Instance;
    public ParticleSystem smokeFX;


    [SerializeField] private float speed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnim;
    private SpriteRenderer myspriteRenderer;
    private KnockBack knockBack;
    private float startingMoveSpeed;

    private AudioManager audioManager;
    private AudioSource sfxSource;
    private bool isWalkingSFXPlaying = false;
    private bool facingLeft = false;
    private bool isDashing = false;

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myspriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<KnockBack>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        sfxSource = audioManager.transform.Find("SFX").GetComponent<AudioSource>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash() ;

        startingMoveSpeed = speed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        if (playerControls != null)
            playerControls.Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
            playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
        HandleSmokeFX();
        HandleWalkSFX();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void HandleWalkSFX()
    {
        bool isMoving = movement.sqrMagnitude > 0.1f;

        if (isMoving && !isDashing)
        {
            if (!sfxSource.isPlaying)
            {
                sfxSource.PlayOneShot(audioManager.walk, 0.5f);
                StartCoroutine(WalkSFXRoutine());
            }
        }
    }

    private IEnumerator WalkSFXRoutine()
    {
        isWalkingSFXPlaying = true;
        while (movement.sqrMagnitude > 0.1f && !isDashing)
        {
            sfxSource.PlayOneShot(audioManager.walk, 0.5f);
            yield return new WaitForSeconds(0.4f); 
        }
        isWalkingSFXPlaying = false;
    }

    private void HandleSmokeFX()
    {
        bool isMoving = movement.sqrMagnitude > 0.1f;

        if (isMoving || isDashing)
        {
            if (!smokeFX.isPlaying)
            {
                smokeFX.Play();
            }
        }
        else
        {
            if (smokeFX.isPlaying)
            smokeFX.Stop();
        }
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnim.SetFloat("moveX", movement.x);
        myAnim.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockBack.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (speed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            myspriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            myspriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            isDashing = true;
            audioManager.PlaySFX(audioManager.dash);
            speed *= dashSpeed;
            myTrailRenderer.emitting = true;
            var emission = smokeFX.emission;
            emission.rateOverTime = 50;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        speed = startingMoveSpeed; 
        myTrailRenderer.emitting = false;
        var emission = smokeFX.emission;
        emission.rateOverTime = 30;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
