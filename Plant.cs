using UnityEngine;

public class Plant : MonoBehaviour
{
    public float height;          // visina biljke
    public string revealedWord;   // rijec koja se pojavi nakon zalijevanja

    [HideInInspector]
    public bool isWatered = false; // je li biljka već zalivena
}
