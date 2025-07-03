using UnityEngine;

public class MapTester : MonoBehaviour
{
    void Start()
    {
        MapGenerator gen = new MapGenerator();
        gen.Generate(25);
    }

}
