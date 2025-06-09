// Updated Juraj's script with control inversion
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sneakSpeed = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public Transform playerCamera;
    public float lookSpeed = 2f;
    private float cameraPitch = 0f;

    [Header("Headbob")]
    public float bobFrequency = 10f;
    public float bobAmplitude = 0.05f;
    private Vector3 defaultCamLocalPos;


    private CharacterController cc;
    private Vector3 velocity;
    private float controlRotationOffset = 0f;
    private int inputRotation = 0; // 0, 90, 180, 270 stupnjeva



    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        defaultCamLocalPos = playerCamera.localPosition;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
    }

    public void RotateControls(float degrees)
    {
        controlRotationOffset += degrees;
        if (controlRotationOffset >= 360f) controlRotationOffset -= 360f;
    }

    void Move()
    {
        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? sneakSpeed : walkSpeed;

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Rotiraj input prema inputRotation
        input = Quaternion.Euler(0, inputRotation, 0) * input;

        Vector3 move = transform.right * input.x + transform.forward * input.z;
        move *= moveSpeed;

        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (cc.isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = move + Vector3.up * velocity.y;
        cc.Move(finalMove * Time.deltaTime);

        // Headbob
        if (cc.isGrounded && move.magnitude > 0.1f)
        {
            float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            playerCamera.localPosition = defaultCamLocalPos + new Vector3(0, bobOffset, 0);
        }
        else
        {
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, defaultCamLocalPos, Time.deltaTime * 5f);
        }
    }

    public void RotateInputControls()
    {
        inputRotation += 90;
        if (inputRotation >= 360)
            inputRotation = 0;
    }

    public void ResetInputControls()
    {
        inputRotation = 0;
    }




}
