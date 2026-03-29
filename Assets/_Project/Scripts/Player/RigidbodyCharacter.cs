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
    [SerializeField] private PlayerAudio playerAudio;
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
        // Inizializzazione del Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Disabilita root motion per l'Animator
        if (animator != null)
            animator.applyRootMotion = false;

        // Controlli di sicurezza sui riferimenti
        Debug.Assert(groundChecker != null, "GroundChecker non assegnato!");
        Debug.Assert(cameraTransform != null, "CameraTransform non assegnato!");
    }

    void Update()
    {
        // Controllo se il personaggio è a terra
        HandleGroundCheck();

        // Lettura input e movimento relativo alla camera
        HandleMovementInput();

        // Rotazione del personaggio verso la direzione di movimento
        HandleRotation();

        // Aggiornamento parametri dell'Animator
        HandleAnimation();

        // Gestione salto
        HandleJump();
    }

    void FixedUpdate()
    {
        // Applica la velocità calcolata al Rigidbody
        ApplyMovement();
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

        // Se lo stato cambia, invia l'evento
        if (wasGrounded != isGrounded)
            OnIsGrounded?.Invoke(isGrounded);

        if (animator != null)
            animator.SetBool("IsGrounded", isGrounded);
    }

    // -------------------- INPUT --------------------

    void HandleMovementInput()
    {
        // Lettura input della tastiera (input continuo, non più discreto)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calcolo direzione rispetto alla camera (vera, non pivot del player)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // Movimento relativo alla camera
        Vector3 rawInput = camForward * v + camRight * h;
        rawInput = rawInput.normalized;

        // Smoothing dell'input
        // Se eravamo fermi e c'è input, forziamo subito l'input per uscire da Idle
        if (smoothedMoveInput.magnitude < 0.01f && rawInput.magnitude > 0.01f)
        {
            smoothedMoveInput = rawInput;
        }
        else
        {
            smoothedMoveInput = Vector3.SmoothDamp(
                smoothedMoveInput,
                rawInput,
                ref moveVelocity,
                inputSmoothTime
            );
        }

        // Imposta a zero input molto piccoli per evitare problemi
        if (smoothedMoveInput.magnitude < 0.01f)
            smoothedMoveInput = Vector3.zero;

        // Aggiorna evento con velocità orizzontale
        OnUpdateHorizontalSpeed?.Invoke(smoothedMoveInput.magnitude);
    }

    // -------------------- ROTATION --------------------

    void HandleRotation()
    {
        // Ruota il personaggio verso la direzione di movimento
        if (smoothedMoveInput.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(smoothedMoveInput);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // -------------------- MOVEMENT --------------------

    void ApplyMovement()
    {
        // Applica la velocità al Rigidbody mantenendo la componente verticale
        Vector3 velocity = smoothedMoveInput * moveSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;

        // Blocca rotazioni indesiderate generate dalla fisica
        rb.angularVelocity = Vector3.zero;
    }

    // -------------------- JUMP --------------------

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Imposta la velocità verticale per saltare
            rb.velocity = new Vector3(
                rb.velocity.x,
                Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y),
                rb.velocity.z
            );

            // Trigger animazione salto
            if (animator != null)
                animator.SetTrigger("Jump");

            // Riproduce audio salto se presente
            playerAudio?.PlayJump();

            // Invoca evento di salto
            OnJump?.Invoke();
        }
    }

    // -------------------- ANIMATION --------------------

    void HandleAnimation()
    {
        if (animator == null) return;

        // Uso della velocità per gestire le animazioni (al posto di MoveX / MoveZ)
        float speed = smoothedMoveInput.magnitude;

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    // -------------------- DEBUG --------------------

    private void OnDrawGizmos()
    {
        // Disegna una sfera per visualizzare il ground checker
        if (groundChecker == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundDistance);
    }
}