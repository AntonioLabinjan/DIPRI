using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TripPickup : MonoBehaviour
{
    public string creepyMessage = "GAME FAILED: YOU ARE TRAPPED...\nCAN YOU ESCAPE THE MADNESS?";

    private static bool tripUIExists = false; // trajna posljedica
    private GameObject canvasGO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !tripUIExists)
        {
            Debug.Log("[TripPickup] Player triggered Gembac trip.");
            tripUIExists = true;
            CreatePersistentUI();
            Destroy(gameObject);
        }
    }

    private void CreatePersistentUI()
    {
        Debug.Log("[TripPickup] Creating persistent UI.");

        // Kreiranje Canvas-a
        canvasGO = new GameObject("TripCanvasPersistent");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10000;

        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        // Provjera EventSystema
        if (EventSystem.FindFirstObjectByType<EventSystem>() == null)
        {
            Debug.Log("[TripPickup] No EventSystem found, creating one.");
            GameObject eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<EventSystem>();
            eventSystemGO.AddComponent<StandaloneInputModule>();
        }
        else
        {
            Debug.Log("[TripPickup] EventSystem already exists.");
        }

        // Kreiranje teksta
        GameObject textGO = new GameObject("CreepyText");
        textGO.transform.SetParent(canvasGO.transform, false);
        Text messageText = textGO.AddComponent<Text>();

        messageText.text = creepyMessage;
        messageText.fontSize = 18; // manji font
        messageText.alignment = TextAnchor.UpperLeft;
        messageText.color = Color.red;
        messageText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        RectTransform textRT = messageText.rectTransform;
        textRT.anchorMin = new Vector2(0f, 1f);
        textRT.anchorMax = new Vector2(0f, 1f);
        textRT.pivot = new Vector2(0f, 1f);
        textRT.anchoredPosition = new Vector2(10f, -10f); // mala margina od gornjeg lijevog kuta
        textRT.sizeDelta = new Vector2(300f, 100f); // dimenzije
    }
}
