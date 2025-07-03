using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Plant> plants;

    public Camera playerCamera; 

    private int currentIndex = 0;

    void Start()
    {
        plants.Sort((a, b) => a.height.CompareTo(b.height));
        Debug.Log("[QuestManager] Quest pocinje. Zalivaj biljke od najnize do najvise.");

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 4f))
            {
                Plant lookedAtPlant = hit.collider.GetComponent<Plant>();
                if (lookedAtPlant != null && !lookedAtPlant.isWatered)
                {
                    TryWaterPlant(lookedAtPlant);
                }
            }
        }
    }

    void TryWaterPlant(Plant plant)
    {
        if (plants[currentIndex] == plant)
        {
            plant.isWatered = true;
            Debug.Log($"[QuestManager] Zalio/la si biljku s rijecju: {plant.revealedWord}");

            currentIndex++;

            if (currentIndex >= plants.Count)
            {
                StartCoroutine(OnQuestComplete());
            }
        }
        else
        {
            Debug.Log("[QuestManager] Krivi redoslijed! Pokusaj ponovno.");
            StartCoroutine(SimpleScaryEffect());
        }
    }

    IEnumerator SimpleScaryEffect()
    {
        Debug.Log("[QuestManager] Mrak te proguta... Pogrijesio/la si.");

        float blinkDuration = 1f;
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            if (playerCamera != null)
                playerCamera.enabled = false;

            yield return new WaitForSeconds(0.1f);

            if (playerCamera != null)
                playerCamera.enabled = true;

            yield return new WaitForSeconds(0.2f);

            elapsed += 0.3f; // 0.1 + 0.2
        }

        // Reset biljki
        currentIndex = 0;
        foreach (var p in plants)
        {
            p.isWatered = false;
        }

        Debug.Log("[QuestManager] Quest resetiran.");
    }


    IEnumerator OnQuestComplete()
    {
        Debug.Log("[QuestManager] Quest zavrsio: You should leave hotel.");
        yield return new WaitForSeconds(3f);
        // Daljnja logika nakon zavrsetka questa
    }
}
