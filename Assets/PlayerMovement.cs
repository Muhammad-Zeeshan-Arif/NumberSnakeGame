using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    public float forwardSpeed = 8f;

    [Header("Horizontal Movement")]
    public float horizontalSpeed = 10f; // tweak for mobile sensitivity
    public float horizontalLimit = 4f; // Width boundary

    private float horizontalInput;

    private bool hasStarted = false;

    void Update()
    {
        // Start on first tap / click
        if (!hasStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hasStarted = true;
            }
        }

        if (hasStarted)
        {
            HandleInput();
            Move();
        }
    }

    private void HandleInput()
    {
#if UNITY_EDITOR
        // Editor: keyboard A/D or Arrow keys
        horizontalInput = Input.GetAxis("Horizontal") * horizontalSpeed;
#else
        // Mobile: swipe / touch drag
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Convert swipe delta to world movement
            horizontalInput = (touch.deltaPosition.x / Screen.width) * horizontalSpeed * 60f; // 60f = frame rate compensation
        }
        else
        {
            horizontalInput = 0f;
        }
#endif
    }

    private void Move()
    {
        // Forward movement
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

        // Horizontal movement
        Vector3 newPosition = transform.position;
        newPosition.x += horizontalInput * Time.deltaTime; // << horizontalSpeed already applied in HandleInput

        // Clamp inside path
        newPosition.x = Mathf.Clamp(newPosition.x, -horizontalLimit, horizontalLimit);

        transform.position = newPosition;
    }
}
