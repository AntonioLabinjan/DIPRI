using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Plant> plants;

    private int currentIndex = 0;

    void Start()
    {
        plants.Sort((a, b) => a.height.CompareTo(b.height));
        Debug.Log("[QuestManager] Quest pocinje. Zalivaj biljke od najnize do najvise.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 4f)) // 4f = maksimalna udaljenost
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
            ResetQuest();
        }
    }

    void ResetQuest()
    {
        currentIndex = 0;
        foreach (var p in plants)
        {
            p.isWatered = false;
        }
        Debug.Log("[QuestManager] Quest resetiran.");
    }

    IEnumerator OnQuestComplete()
    {
        Debug.Log("[QuestManager] Quest završio: You should leave hotel.");
        yield return new WaitForSeconds(3f);
        // Daljnja logika nakon završetka questa
    }
}
