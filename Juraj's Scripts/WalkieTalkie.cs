using UnityEngine;

[RequireComponent(typeof(Pickable))]
public class WalkieTalkieUsable : MonoBehaviour, IUsable
{
    public AudioSource walkieAudio;
    private Pickable pickable;

    private void Start()
    {
        pickable = GetComponent<Pickable>();

        if (pickable == null)
        {
            Debug.LogError("WalkieTalkie: No Pickable found!");
            return;
        }

        if (pickable.isActive)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void Use()
    {
        if (walkieAudio == null || pickable == null)
        {
            Debug.LogWarning("WalkieTalkie: Missing components.");
            return;
        }

        if (walkieAudio.isPlaying)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }

        pickable.isActive = walkieAudio.isPlaying;
    }

    private void TurnOn()
    {
        walkieAudio.loop = true;
        walkieAudio.Play();
    }

    private void TurnOff()
    {
        walkieAudio.Stop();
    }
}
