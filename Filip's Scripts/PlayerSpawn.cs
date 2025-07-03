using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;

    void Start()
    {
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
