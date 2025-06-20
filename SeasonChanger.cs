using UnityEngine;

public class SeasonChanger : MonoBehaviour
{
    public Texture[] seasonTextures;
    public Material windowMaterial;
    public Transform playerCamera;
    public string windowTag = "Window";

    private Texture currentTexture;
    private bool wasLookingAtWindow = false;
    private float cooldown = 2f;
    private float lastChangeTime = -10f;

    void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        bool isLookingAtWindow = false;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag(windowTag))
            {
                isLookingAtWindow = true;

                if (!wasLookingAtWindow && Time.time - lastChangeTime > cooldown)
                {
                    ChangeSeason();
                    lastChangeTime = Time.time;
                }
            }
        }

        wasLookingAtWindow = isLookingAtWindow;
    }

    void ChangeSeason()
    {
        Texture newTexture = currentTexture;

        // Ensure it's different from current
        int safety = 10; // avoid infinite loop if only one texture
        while (newTexture == currentTexture && safety-- > 0)
        {
            newTexture = seasonTextures[Random.Range(0, seasonTextures.Length)];
        }

        currentTexture = newTexture;
        windowMaterial.mainTexture = currentTexture;
    }
}
