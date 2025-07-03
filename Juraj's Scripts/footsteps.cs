using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepsSource;
    public AudioClip[] footstepClips;
    public float stepInterval = 0.5f;

    private float stepTimer = 0f;
    private float jumpSuppressTimer = 0f;
    private float suppressDuration = 1f;

    void Update()
    {
        // If Space is pressed, start suppression
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpSuppressTimer = suppressDuration;
            if (footstepsSource.isPlaying)
                footstepsSource.Stop();
        }

        // Countdown suppress timer
        if (jumpSuppressTimer > 0f)
        {
            jumpSuppressTimer -= Time.deltaTime;
            return; // Suppress footstep sounds
        }

        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                         Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isWalking)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
            if (footstepsSource.isPlaying)
                footstepsSource.Stop();
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        footstepsSource.clip = footstepClips[index];
        footstepsSource.Play();
    }
}
