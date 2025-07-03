using UnityEngine;
using System;
using UnityEngine.Rendering.PostProcessing;

[DisallowMultipleComponent]
public class SanityManager : MonoBehaviour
{
    [Header("Sanity Settings")]
    public int maxSanity = 100;
    public int stepSize = 10;

    [Header("Optional Effects")]
    public PostProcessVolume[] ppVolumes; 
    public AudioClip[] stageSounds;       
    public AudioSource audioSource;      

    public int CurrentSanity { get; private set; }
    public int CurrentStage { get; private set; }

    public event Action<int> OnSanityChanged;
    public event Action<int, int> OnSanityStageChanged; 

    void Awake()
    {
        CurrentSanity = maxSanity;
        CurrentStage = StageForSanity(CurrentSanity);
        UpdateVisualSanityEffects();
        ApplyStageEffects(CurrentStage);
    }

    void Update()
    {
        #if UNITY_EDITOR 
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            ChangeSanity(-5);

        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            ChangeSanity(+5);
        #endif
    }


    public void ChangeSanity(int delta)
    {
        int oldSanity = CurrentSanity;
        CurrentSanity = Mathf.Clamp(CurrentSanity + delta, 0, maxSanity);
        OnSanityChanged?.Invoke(CurrentSanity);

        int newStage = StageForSanity(CurrentSanity);
        if (newStage != CurrentStage)
        {
            int prev = CurrentStage;
            CurrentStage = newStage;
            OnSanityStageChanged?.Invoke(prev, newStage);
            ApplyStageEffects(newStage);
        }
        if (ppVolumes != null)
        {
            UpdateVisualSanityEffects();
        }
        Debug.Log($"Sanity: {CurrentSanity}");
    }

    public void SetSanity(int value)
        => ChangeSanity(value - CurrentSanity);

    int StageForSanity(int sanity)
    {
        return (maxSanity - sanity) / stepSize;
    }

    void ApplyStageEffects(int stage)
    {
        if (audioSource != null && stageSounds != null && stage < stageSounds.Length)
        {
            if (audioSource.clip != stageSounds[stage])
            {
                audioSource.clip = stageSounds[stage];
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        Debug.Log($"[Sanity] Entered stage {stage} (sanity={CurrentSanity})");
    }
    void UpdateVisualSanityEffects()
    {
        float weight = 1f - (CurrentSanity / (float)maxSanity);
        foreach (var v in ppVolumes)
            v.weight = weight;
    }

}
