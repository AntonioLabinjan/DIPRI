using UnityEngine;

public class MazeGoalDetector : MonoBehaviour
{
    private Vector3 startPosition;
    public float moveSpeed = 5f;
    private PlayerController playerController;

    private float elapsedTime = 0f;
    private float timeLimit = 60f;
    private bool hasFailed = false;
    private bool goalReached = false;

    private void Start()
    {
        startPosition = transform.position;
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogError("PlayerController not found on MazeGoalDetector object!");

        elapsedTime = 0f;
    }

    private void Update()
    {
        if (goalReached || hasFailed)
            return; 

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);
        Vector3 move = inputDir.normalized * moveSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeLimit)
        {
            hasFailed = true;
            Debug.Log("Vrijeme isteklo! Quest nije uspio.");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (goalReached || hasFailed)
            return;

        if (other.CompareTag("MazeGoal"))
        {
            goalReached = true;
            Debug.Log("Labirint uspjesno rijesen!");
            Debug.Log("Vrijeme prolaska: " + elapsedTime.ToString("F2") + " sekundi");
            playerController?.ResetInputControls();
        }
        else if (other.CompareTag("DeadlyObstacle"))
        {
            Debug.Log("Osamucen si – kontrole rotirane!");
            playerController?.RotateInputControls();
            ResetToStart();
        }
    }

    private void ResetToStart()
    {
        transform.position = startPosition;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
