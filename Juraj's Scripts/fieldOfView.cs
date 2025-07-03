using UnityEngine;
public class FieldOfView : MonoBehaviour
{
    public float radius = 10f;
    public float angle = 70f;
    public LayerMask maskObstacles;
    public Transform target; 

    public bool CanSeeTarget(out Vector3 lastSeen)
    {
        lastSeen = Vector3.zero;
        Vector3 dir = (target.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dir) > angle * 0.5f) return false;
        if (Vector3.Distance(transform.position, target.position) > radius) return false;

        if (!Physics.Linecast(transform.position, target.position, maskObstacles))
        {
            lastSeen = target.position;
            return true;
        }
        return false;
    }
}
