using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapsuleTeleport : MonoBehaviour
{
    public float teleportDuration = 30f;
    private bool isTeleporting = false;

    private Vector3 originalPlayerPosition;
    private GameObject player;

    private AsyncOperation magicWindowLoadOp;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            Debug.LogError("[CapsuleTeleport] Nema objekta s tagom 'Player'!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting)
            return;

        if (other.gameObject == player)
        {
            StartCoroutine(TeleportRoutine());
        }
    }

    private IEnumerator TeleportRoutine()
    {
        isTeleporting = true;

        originalPlayerPosition = player.transform.position;

        magicWindowLoadOp = SceneManager.LoadSceneAsync("MagicWindow", LoadSceneMode.Additive);
        yield return new WaitUntil(() => magicWindowLoadOp.isDone);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MagicWindow"));

        Debug.Log("[CapsuleTeleport] Igrac teleportiran u MagicWindow.");

        yield return new WaitForSeconds(teleportDuration);

        Scene worldScene = SceneManager.GetSceneByName("World");
        if (!worldScene.IsValid())
        {
            Debug.LogError("[CapsuleTeleport] World scena nije ucitana!");
        }
        else
        {
            SceneManager.SetActiveScene(worldScene);
        }

        AsyncOperation unloadMagic = SceneManager.UnloadSceneAsync("MagicWindow");
        yield return new WaitUntil(() => unloadMagic.isDone);

        // Pomakni poziciju malo, npr. 2 metra naprijed po Z osi (ili odaberi neki drugi smjer)
        Vector3 offset = new Vector3(0f, 0f, 2f);
        player.transform.position = originalPlayerPosition + offset;

        Debug.Log("[CapsuleTeleport] Igrac vracen u World na poziciju sa offsetom.");

        isTeleporting = false;
    }

}
