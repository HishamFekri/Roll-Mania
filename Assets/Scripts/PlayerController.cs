using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private float movementX;
    private float movementY;

    public float speed = 0;

    [Header("Speed Settings")]
    public float maxSpeed = 100f;

    [Header("Gravity Settings")]
    public float gravityMultiplier = 3f;

    // Prevent the ball from launching too high after collisions
    public float maxUpwardSpeed = 8f;

    private int count;

    public TextMeshProUGUI countText;

    [Header("UI Elements")]
    public GameObject winTextObject;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI speedText;

    // Restart button / panel
    public GameObject restartUIObject;

    // Pause menu
    public GameObject pauseMenuObject;
    public TextMeshProUGUI pauseText;

    // Distance of UI from camera
    public float uiOffsetDistance = 2.0f;

    // Main camera
    private Camera mainCamera;

    // Player control
    public bool canControl = true;

    // Sprint control
    private bool isSprinting = false;
    private const float sprintMultiplier = 2f;

    // Gravity control
    private bool isHighGravity = false;

    // Timer variables
    private float gameStartTime;
    private bool gameStarted = false;
    private bool gameEnded = false;

    // Pause control
    private bool isPaused = false;
    private float pauseStartTime;
    private float totalPauseTime = 0f;

    [Header("Audio")]
    public AudioSource pickupAudioSource;
    public AudioSource winAudioSource;
    public AudioSource loseAudioSource;

    [Header("VFX")]
    public GameObject playerHitVFX;
    public GameObject pickupVFX;
    public GameObject enemyDestroyVFX;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;

        count = 0;
        SetCountText();

        // Reset win text
        if (winTextObject != null)
        {
            winTextObject.SetActive(false);

            TextMeshProUGUI winText =
                winTextObject.GetComponent<TextMeshProUGUI>();

            if (winText != null)
            {
                winText.text = "";
            }
        }

        // Hide restart UI
        if (restartUIObject != null)
        {
            restartUIObject.SetActive(false);
        }

        // Hide pause menu
        if (pauseMenuObject != null)
        {
            pauseMenuObject.SetActive(false);
        }

        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(false);
        }

        // Initialize timer
        gameStartTime = Time.time;

        gameStarted = false;
        gameEnded = false;

        isPaused = false;

        totalPauseTime = 0f;

        // Start unpaused
        Time.timeScale = 1f;

        // Reset player state
        canControl = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        // Enable renderer
        MeshRenderer renderer =
            gameObject.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            renderer.enabled = true;
        }

        // Enable collider
        Collider collider =
            gameObject.GetComponent<Collider>();

        if (collider != null)
        {
            collider.enabled = true;
        }

        // Reset timer text
        if (timerText != null)
        {
            timerText.text = "Time: 0.00";
        }

        // Reset speed text
        if (speedText != null)
        {
            speedText.text = "Speed: 0.00";
        }
    }


    void OnMove(InputValue movementValue)
    {
        if (!canControl)
            return;

        Vector2 movementVector =
            movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    void SetCountText()
    {
        if (countText != null)
        {
            countText.text =
                "Count: " + count.ToString();
        }
    }


    void Update()
    {
        // Hide win text if game hasn't ended
        if (
            !gameEnded &&
            !gameStarted &&
            winTextObject != null &&
            winTextObject.activeSelf
        )
        {
            winTextObject.SetActive(false);

            TextMeshProUGUI winText =
                winTextObject.GetComponent<TextMeshProUGUI>();

            if (winText != null)
            {
                winText.text = "";
            }
        }

        // ESC pause / unpause
        if (
            !gameEnded &&
            Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame
        )
        {
            TogglePause();
        }

        // Stop processing input when paused
        if (isPaused)
        {
            return;
        }

        if (canControl)
        {
            // SHIFT = sprint
            isSprinting =
                Keyboard.current != null &&
                Keyboard.current.shiftKey.isPressed;

            // SPACE = stronger gravity
            isHighGravity =
                Keyboard.current != null &&
                Keyboard.current.spaceKey.isPressed;
        }
        else
        {
            isSprinting = false;
            isHighGravity = false;
        }

        // Start timer
        if (!gameStarted && canControl)
        {
            gameStarted = true;
            gameStartTime = Time.time;
        }

        // Update timer
        if (
            gameStarted &&
            !gameEnded &&
            timerText != null
        )
        {
            float elapsedTime =
                Time.time -
                gameStartTime -
                totalPauseTime;

            timerText.text =
                "Time: " +
                elapsedTime.ToString("F2");
        }

        // Update speed display
        if (
            speedText != null &&
            rb != null
        )
        {
            Vector3 velocity = rb.velocity;

            // Only horizontal speed
            velocity.y = 0f;

            float currentSpeed =
                velocity.magnitude;

            speedText.text =
                "Speed: " +
                currentSpeed.ToString("F2");
        }
    }


    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Pause
            Time.timeScale = 0f;

            pauseStartTime =
                Time.realtimeSinceStartup;

            canControl = false;

            if (pauseMenuObject != null)
            {
                pauseMenuObject.SetActive(true);
            }

            if (pauseText != null)
            {
                pauseText.gameObject.SetActive(true);
            }
        }
        else
        {
            // Unpause
            Time.timeScale = 1f;

            float pauseDuration =
                Time.realtimeSinceStartup -
                pauseStartTime;

            totalPauseTime += pauseDuration;

            if (!gameEnded)
            {
                canControl = true;
            }

            if (pauseMenuObject != null)
            {
                pauseMenuObject.SetActive(false);
            }

            if (pauseText != null)
            {
                pauseText.gameObject.SetActive(false);
            }
        }
    }


    private void FixedUpdate()
    {
        if (canControl)
        {
            // Camera-relative movement
            Vector3 cameraForward =
                mainCamera.transform.forward;

            Vector3 cameraRight =
                mainCamera.transform.right;

            // Remove vertical movement
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 movement =
                (cameraForward * movementY) +
                (cameraRight * movementX);

            // Sprint
            float currentSpeed = speed;

            if (isSprinting)
            {
                currentSpeed *= sprintMultiplier;
            }

            // Apply movement force
            rb.AddForce(
                movement * currentSpeed
            );

            /*
             * SPACE GRAVITY
             *
             * Instead of changing Rigidbody.mass,
             * we apply additional downward acceleration.
             *
             * This gives the player more control
             * without changing the physics properties
             * of the Rigidbody.
             */
            if (isHighGravity)
            {
                Vector3 extraGravity =
                    Physics.gravity *
                    (gravityMultiplier - 1f);

                rb.AddForce(
                    extraGravity,
                    ForceMode.Acceleration
                );
            }


            /*
             * Prevent extreme upward launches.
             *
             * If a collision or ramp launches
             * the ball upward too quickly,
             * clamp the vertical velocity.
             */
            Vector3 currentVelocity =
                rb.velocity;

            if (
                currentVelocity.y >
                maxUpwardSpeed
            )
            {
                currentVelocity.y =
                    maxUpwardSpeed;

                rb.velocity =
                    currentVelocity;
            }


            /*
             * Limit horizontal speed.
             */
            Vector3 horizontalVelocity =
                rb.velocity;

            horizontalVelocity.y = 0f;

            if (
                horizontalVelocity.magnitude >
                maxSpeed
            )
            {
                horizontalVelocity =
                    horizontalVelocity.normalized *
                    maxSpeed;

                rb.velocity =
                    new Vector3(
                        horizontalVelocity.x,
                        rb.velocity.y,
                        horizontalVelocity.z
                    );
            }
        }
        else
        {
            movementX = 0;
            movementY = 0;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // Finish arch
        if (
            other.gameObject.name.Contains("FinishArch") ||
            other.gameObject.name == "FinishArch"
        )
        {
            EndGameAtFinish();
            return;
        }

        // Pickup
        if (
            other.gameObject.CompareTag("PickUp")
        )
        {
            if (pickupVFX != null)
            {
                GameObject vfx =
                    Instantiate(
                        pickupVFX,
                        other.transform.position,
                        Quaternion.identity
                    );

                Destroy(vfx, 3f);
            }

            other.gameObject.SetActive(false);

            count += 1;

            SetCountText();

            pickupAudioSource?.Play();
        }

        // Win after collecting 11 objects
        if (count >= 11)
        {
            gameEnded = true;

            if (winTextObject != null)
            {
                winTextObject.SetActive(true);

                winTextObject
                    .GetComponent<TextMeshProUGUI>()
                    .text = "You Win!";
            }

            PositionUItoCamera();

            if (restartUIObject != null)
            {
                restartUIObject.SetActive(true);
            }

            GameObject enemy =
                GameObject.FindGameObjectWithTag(
                    "Enemy"
                );

            if (enemy != null)
            {
                if (enemyDestroyVFX != null)
                {
                    GameObject vfx =
                        Instantiate(
                            enemyDestroyVFX,
                            enemy.transform.position,
                            Quaternion.identity
                        );

                    Destroy(vfx, 3f);
                }

                Destroy(enemy);

                rb.isKinematic = true;
                canControl = false;
            }

            winAudioSource?.Play();
        }
    }


    private void OnCollisionEnter(
        Collision collision
    )
    {
        // Finish arch
        if (
            collision.gameObject.name.Contains(
                "FinishArch"
            ) ||
            collision.gameObject.name ==
            "FinishArch"
        )
        {
            EndGameAtFinish();
            return;
        }

        // Enemy collision
        if (
            collision.gameObject.CompareTag(
                "Enemy"
            )
        )
        {
            if (playerHitVFX != null)
            {
                Instantiate(
                    playerHitVFX,
                    transform.position,
                    Quaternion.identity
                );
            }

            MeshRenderer renderer =
                gameObject.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                renderer.enabled = false;
            }

            Collider collider =
                gameObject.GetComponent<Collider>();

            if (collider != null)
            {
                collider.enabled = false;
            }

            rb.isKinematic = true;

            canControl = false;

            gameEnded = true;

            if (winTextObject != null)
            {
                winTextObject.SetActive(true);

                winTextObject
                    .GetComponent<TextMeshProUGUI>()
                    .text = "You Lose!";
            }

            PositionUItoCamera();

            if (restartUIObject != null)
            {
                restartUIObject.SetActive(true);
            }

            loseAudioSource?.Play();
        }
    }


    void EndGameAtFinish()
    {
        // Prevent multiple calls
        if (gameEnded)
            return;

        gameEnded = true;

        // Stop player
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        canControl = false;

        // Calculate final time
        float finalTime =
            Time.time -
            gameStartTime -
            totalPauseTime;

        if (winTextObject != null)
        {
            winTextObject.SetActive(true);

            winTextObject
                .GetComponent<TextMeshProUGUI>()
                .text =
                "You Win!\nTime: " +
                finalTime.ToString("F2") +
                "s";
        }

        PositionUItoCamera();

        if (restartUIObject != null)
        {
            restartUIObject.SetActive(true);
        }

        winAudioSource?.Play();
    }


    void PositionUItoCamera()
    {
        if (
            restartUIObject != null &&
            mainCamera != null
        )
        {
            restartUIObject.transform.position =
                mainCamera.transform.position +
                (
                    mainCamera.transform.forward *
                    uiOffsetDistance
                );

            restartUIObject.transform.rotation =
                mainCamera.transform.rotation;
        }
    }


    public void SetPlayerControl(
        bool durum
    )
    {
        canControl = durum;

        if (!durum)
        {
            movementX = 0;
            movementY = 0;
        }
    }


    public void ResetGame()
    {
        // Reset game state
        gameEnded = false;
        gameStarted = false;
        isPaused = false;

        totalPauseTime = 0f;
        gameStartTime = Time.time;

        count = 0;

        SetCountText();

        // Hide win text
        if (winTextObject != null)
        {
            winTextObject.SetActive(false);

            TextMeshProUGUI winText =
                winTextObject
                    .GetComponent<TextMeshProUGUI>();

            if (winText != null)
            {
                winText.text = "";
            }
        }

        // Hide restart UI
        if (restartUIObject != null)
        {
            restartUIObject.SetActive(false);
        }

        // Hide pause menu
        if (pauseMenuObject != null)
        {
            pauseMenuObject.SetActive(false);
        }

        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(false);
        }

        // Reset player
        canControl = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        // Enable renderer
        MeshRenderer renderer =
            gameObject.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            renderer.enabled = true;
        }

        // Enable collider
        Collider collider =
            gameObject.GetComponent<Collider>();

        if (collider != null)
        {
            collider.enabled = true;
        }

        // Reset UI
        if (timerText != null)
        {
            timerText.text = "Time: 0.00";
        }

        if (speedText != null)
        {
            speedText.text = "Speed: 0.00";
        }

        // Unpause
        Time.timeScale = 1f;
    }
}