using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorRoom : MonoBehaviour
{
    public string roomNumber;


    public QuestManager2 questManager2;   // suitcase delivery
    public QuestManager3 questManager3;    // bag between the lines

    [Header("Special Story Flags")]
    public bool isRoom504 = false;
    public bool isRoom405 = false;
    public bool isElevator = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (isRoom504 && questManager3 != null)
                    {
                        KnockRoom504();
                    }
                    else if (isRoom405 && questManager3 != null)
                    {
                        KnockRoom405();
                    }  
                    else if (questManager2 != null)
                    {
                        HandleSuitcaseDelivery();
                    }
                }
            }
        }
    }


    void HandleSuitcaseDelivery()
    {
        if (roomNumber != questManager2.currentRoomNumber)
        {
            questManager2.ClickedRoomNum.text = $"Room: {roomNumber}";
            questManager2.StopAllCoroutines();
            questManager2.StartCoroutine(questManager2.FadeOutClickedText(1.2f));
        }
        else
        {
            questManager2.Delivered.text = "Suitcase delivered!";
            questManager2.StopAllCoroutines();
            questManager2.StartCoroutine(questManager2.HideDeliveredText(1.2f));
            questManager2.FoundRoom();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (questManager3.visited504 && questManager3.visited405 && questManager3.heardGunshot && !questManager3.questFinished)
            {
                questManager3.EnterElevator();
            }
        }
    }

    void KnockRoom504()
    {
        questManager3.KnockRoom504();
    }

    void KnockRoom405()
    {
        questManager3.KnockRoom405();
    }

}




