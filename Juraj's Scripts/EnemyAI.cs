using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    enum State { Patrol, Alert, Chase }

    [Header("General")]
    public NavMeshAgent agent;
    public FieldOfView fov;
    public float patrolRadius = 8f;
    public float alertSearchTime = 45f;    

    [Header("Speeds")]
    public float patrolSpeed = 2;
    public float alertSpeed = 3;
    public float chaseSpeed = 5;

    State state = State.Patrol;
    float alertTimer;
    Vector3 lastInterestingPos;

    void Start() => PickPatrolPoint();

    void Update()
    {
        switch (state)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Alert: UpdateAlert(); break;
            case State.Chase: UpdateChase(); break;
        }
    }

    void UpdatePatrol()
    {
        if (Arrived()) PickPatrolPoint();

        if (fov.CanSeeTarget(out var seen))
            SwitchToChase(seen);
        else if (HeardSomething(out var soundPos))
            SwitchToAlert(soundPos);
    }
    void PickPatrolPoint()
    {
        Vector3 patrolPoint = GetRandomNavMeshLocation(patrolRadius);
        agent.SetDestination(patrolPoint);
        agent.speed = patrolSpeed;
    }


    void SwitchToAlert(Vector3 pos)
    {
        state = State.Alert;
        lastInterestingPos = pos;
        agent.speed = alertSpeed;
        agent.SetDestination(lastInterestingPos);
        alertTimer = alertSearchTime;
    }
    void UpdateAlert()
    {
        alertTimer -= Time.deltaTime;
        if (fov.CanSeeTarget(out var seen))
            SwitchToChase(seen);
        else if (alertTimer <= 0f)
            state = State.Patrol;
        else if (Arrived())             
            agent.SetDestination(lastInterestingPos + Random.insideUnitSphere * 2f);
    }

    void SwitchToChase(Vector3 pos)
    {
        state = State.Chase;
        agent.speed = chaseSpeed;
        agent.SetDestination(pos);
    }
    void UpdateChase()
    {
        agent.SetDestination(fov.target.position);
        if (!fov.CanSeeTarget(out var seen))
        {
            SwitchToAlert(fov.target.position);
        }
    }

    bool Arrived() => !agent.pathPending &&
                      agent.remainingDistance <= agent.stoppingDistance;

    bool HeardSomething(out Vector3 pos)
    {
        pos = Vector3.zero;
        return false;
    }
    Vector3 GetRandomNavMeshLocation(float radius)
    {
        for (int i = 0; i < 30; i++) 
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection.y = 0;

            Vector3 randomPoint = transform.position + randomDirection;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                return hit.position;
        }

        // Fallback
        return transform.position;
    }

}
