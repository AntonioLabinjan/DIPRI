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

        // Ucitaj MagicWindow scenu additivno preko World scene
        magicWindowLoadOp = SceneManager.LoadSceneAsync("MagicWindow", LoadSceneMode.Additive);
        yield return new WaitUntil(() => magicWindowLoadOp.isDone);

        // Iskljuci aktivnost World scene da bismo "presli" u MagicWindow
        Scene worldScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MagicWindow"));

        // Mozes po zelji ovdje teleportirati igraca na odredenu poziciju u MagicWindow sceni
        // player.transform.position = new Vector3(x, y, z);

        // cekaj 30 sekundi u MagicWindow sceni
        yield return new WaitForSeconds(teleportDuration);

        // Aktiviraj opet World scenu i deaktiviraj MagicWindow scenu
        SceneManager.SetActiveScene(worldScene);

        // Izbaci (unload) MagicWindow scenu
        AsyncOperation unloadMagic = SceneManager.UnloadSceneAsync("MagicWindow");
        yield return new WaitUntil(() => unloadMagic.isDone);

        // Vrati igraca na pocetnu poziciju u World sceni
        player.transform.position = originalPlayerPosition;

        isTeleporting = false;
    }
}
