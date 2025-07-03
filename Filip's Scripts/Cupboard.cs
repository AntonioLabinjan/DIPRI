using UnityEngine;

public class Cupboard : MonoBehaviour
{
    public bool containsTray = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    public bool isOpen = false;
    public float openAngle = -90f;
    public float openSpeed = 2f;
    public Vector3 hingeAxis = Vector3.up;

    public QuestManager questManager;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, hingeAxis);
    }
    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                Cupboard cupboard = hit.collider.GetComponentInParent<Cupboard>();
                if (cupboard == this)
                {
                    HandleClick();
                }
            }
        }
    }
    void HandleClick()
    {
        if (!isOpen)
        {
            isOpen = true;

            if (containsTray)
            {
                Debug.Log("Tray found!");
                questManager.FoundTray();
            }
            else
            {
                Debug.Log("No tray inside cabinet");
            }
        }
        else
        {
            isOpen = false;
        }
    }
}
