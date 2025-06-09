using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    public float boostDuration = 30f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ActivateSpeedBoost(boostDuration);
            Destroy(gameObject); // Ukloni pickup nakon uzimanja
        }
    }
}
