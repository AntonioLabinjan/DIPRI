using UnityEngine;

public class MazeGoalDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MazeGoal"))
        {
            Debug.Log("Labirint uspješno riješen");
        }
    }
}
