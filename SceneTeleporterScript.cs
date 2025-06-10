// prehiti nas iz scene u scenu, ovisno kako definiramo na pickupu
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporterScript : MonoBehaviour
{
    public string sceneToLoad;
    [TextArea]
    public string sceneLogMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(sceneLogMessage);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
