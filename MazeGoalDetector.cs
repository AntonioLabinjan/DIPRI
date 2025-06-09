using UnityEngine;

public class MazeGoalDetector : MonoBehaviour
{
    private Vector3 startPosition;
    public float moveSpeed = 5f;
    private PlayerController playerController;

    private void Start()
    {
        startPosition = transform.position;
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogError("PlayerController not found on MazeGoalDetector object!");
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);
        Vector3 move = inputDir.normalized * moveSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MazeGoal"))
        {
            Debug.Log("Labirint uspješno riješen");
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
