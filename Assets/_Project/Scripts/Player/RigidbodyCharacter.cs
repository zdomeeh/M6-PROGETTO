using UnityEngine;
using UnityEngine.Events;

public class RigidbodyCharacter : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float inputSmoothTime = 0.1f;

    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    private Vector3 smoothedMoveInput;
    private Vector3 moveVelocity;
    private bool isGrounded;

    public UnityEvent<float> OnUpdateHorizontalSpeed;
    public UnityEvent<bool> OnIsGrounded;
    public UnityEvent OnJump;

    void Awake()
    {
        // Inizializza Rigidbody e blocca rotazioni indesiderate
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Disabilita root motion per animator
        if (animator != null)
            animator.applyRootMotion = false;

        // Controllo riferimenti importanti
        Debug.Assert(groundChecker != null, "GroundChecker non assegnato!");
        Debug.Assert(cameraTransform != null, "CameraTransform non assegnato!");
    }

    void Update()
    {
        HandleGroundCheck();     // verifica se il personaggio e' a terra
        HandleMovementInput();   // legge input e calcola direzione movimento
        HandleRotation();        // ruota verso direzione movimento
        HandleAnimation();       // aggiorna animazioni
        HandleJump();            // gestisce salto
    }

    void FixedUpdate()
    {
        ApplyMovement();         // applica la velocità al Rigidbody
    }

    // -------------------- GROUND --------------------
    void HandleGroundCheck()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics.CheckSphere(
            groundChecker.position,
            groundDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        // Se lo stato cambia, invia evento
        if (wasGrounded != isGrounded)
            OnIsGrounded?.Invoke(isGrounded);

        if (animator != null)
            animator.SetBool("IsGrounded", isGrounded);
    }

    // -------------------- INPUT --------------------
    void HandleMovementInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calcola direzione movimento relativa alla camera
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 rawInput = camForward * v + camRight * h;
        rawInput = rawInput.normalized;

        // Smoothing input per movimenti fluidi
        if (smoothedMoveInput.magnitude < 0.01f && rawInput.magnitude > 0.01f)
            smoothedMoveInput = rawInput; // forza input se eravamo fermi
        else
            smoothedMoveInput = Vector3.SmoothDamp(smoothedMoveInput, rawInput, ref moveVelocity, inputSmoothTime);

        if (smoothedMoveInput.magnitude < 0.01f)
            smoothedMoveInput = Vector3.zero;

        OnUpdateHorizontalSpeed?.Invoke(smoothedMoveInput.magnitude);
    }

    // -------------------- ROTATION --------------------
    void HandleRotation()
    {
        if (smoothedMoveInput.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(smoothedMoveInput);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // -------------------- MOVEMENT --------------------
    void ApplyMovement()
    {
        Vector3 velocity = smoothedMoveInput * moveSpeed;
        velocity.y = rb.velocity.y; // mantieni velocità verticale (gravità / salto)
        rb.velocity = velocity;
        rb.angularVelocity = Vector3.zero; // blocca rotazioni indesiderate
    }

    // -------------------- JUMP --------------------
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(
                rb.velocity.x,
                Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y),
                rb.velocity.z
            );

            if (animator != null) animator.SetTrigger("Jump");
            AudioManager.Instance?.PlayPlayerJump();
            OnJump?.Invoke();
        }
    }

    // -------------------- ANIMATION --------------------
    void HandleAnimation()
    {
        if (animator == null) return;
        float speed = smoothedMoveInput.magnitude;
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    // -------------------- DEBUG --------------------
    private void OnDrawGizmos()
    {
        if (groundChecker == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundDistance); // mostra ground checker
    }
}