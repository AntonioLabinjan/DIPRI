using UnityEngine;

public class UnlockedDoor : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;
    public float speed = 2f;
    bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openAngle : 0f));
    }

    System.Collections.IEnumerator RotateDoor(float target)
    {
        float startY = transform.localEulerAngles.y;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float y = Mathf.LerpAngle(startY, target, t);
            transform.localEulerAngles = new Vector3(0, y, 0);
            yield return null;
        }
    }
}
