using System.Collections.Generic;
using System.Linq;
using Movement;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class HotelGenerator : MonoBehaviour
{
    [Header("Core Database")]
    public ModuleDatabase moduleDb;     
    public ModuleDefinition startModule; 

    [Header("Generation Settings")]
    public int initialModules = 5;

    HashSet<Vector3Int> occupied = new HashSet<Vector3Int>();
    Queue<SocketRequest> frontier = new Queue<SocketRequest>();
    List<Bounds> placedBounds = new List<Bounds>();
    public NavMeshSurface navMeshSurface;

    List<ModuleDefinition> hallwayPool;
    List<ModuleDefinition> roomPool;


    struct SocketRequest
    {
        public Vector3 worldCell;
        public Vector3 normal;
        public string socketTag;
        public Quaternion rotation;
    }

    const float cellUnit = 0.5f;        
    static Vector3Int ToGrid(Vector3 pos)
    {
        return Vector3Int.RoundToInt(pos / cellUnit);   
    }



    List<GameObject> spawnedModules = new List<GameObject>();
    struct Candidate
    {
        public ModuleDefinition def;
        public Quaternion rotation;
        public SocketAnchor anchor;
        public Vector3 origin;
    }

    bool stepMode = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            spawnedModules.Clear();
            occupied.Clear();
            frontier.Clear();
            placedBounds.Clear();
            GenerateInitialLayout();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            stepMode = true;
            if (frontier.Count > 0)
            {
                var req = frontier.Dequeue();
                TryExpand(req);
            }
        }
    }

    void Start()
    {
        hallwayPool = moduleDb.modules
             .Where(m => m.prefab.CompareTag("Hall"))  
             .ToList();

        roomPool = moduleDb.modules
                     .Where(m => m.prefab.CompareTag("Room")    
                           || m.prefab.CompareTag("Reception"))
                     .ToList();

        GenerateInitialLayout();
    }


    void TryExpand(SocketRequest req)
    {
        var candidates = new List<Candidate>();

        List<ModuleDefinition> pool = Random.value < 0.75f ? hallwayPool : roomPool;

        foreach (var def in pool.OrderBy(_ => Random.value))
        {
            if (def == startModule) continue;

            foreach (var angle in new[] { 0, 90, 180, 270 }.OrderBy(_ => Random.value))
            {
                var rot = Quaternion.Euler(0, angle, 0);

                foreach (var anchor in GetAnchors(def))
                {
                    //if (anchor.socketTag != req.socketTag) continue;

                    Vector3 testNormal = rot * (anchor.transform.localRotation * Vector3.forward);
                    if (Vector3.Angle(testNormal, -req.normal) > 5f)
                        continue;

                    Vector3 worldOff = rot * anchor.transform.localPosition;
                    Vector3 origin = SnapHalf(req.worldCell - worldOff);

                    var candBounds = MakeWorldBounds(def, origin, rot);
                    if (Overlaps(candBounds)) continue;

                    candidates.Add(new Candidate
                    {
                        def = def,
                        rotation = rot,
                        anchor = anchor,
                        origin = origin
                    });
                }
            }
        }

        if (candidates.Count > 0)
        {
            var pick = candidates[Random.Range(0, candidates.Count)];
            Debug.Log($"[Gen] Placing {pick.def.name} at {pick.origin:F2} " +
                      $"rot {pick.rotation.eulerAngles.y}° on {pick.anchor.name}");
            SpawnModule(pick.def, pick.origin, pick.rotation, pick.anchor.name);
        }
        else
        {
            Debug.Log($"[Gen] ❌ No fitting module for socket at {req.worldCell:F2} " +
                      $"(Tag:{req.socketTag} Dir:{req.normal})");
        }
    }

    void SpawnModule(ModuleDefinition def,
                     Vector3 origin,
                     Quaternion rot,
                     string usedAnchorName)
    {
        GameObject instance = Instantiate(def.prefab, origin, rot, transform);
        spawnedModules.Add(instance);
        placedBounds.Add(MakeWorldBounds(def, origin, rot));


        for (int x = 0; x < def.size.x; x++)
            for (int y = 0; y < def.size.y; y++)
                for (int z = 0; z < def.size.z; z++)
                {
                    Vector3 local = new Vector3(x, y, z);
                    Vector3 world = origin + RotateCell(local, rot, def.size);
                    occupied.Add(ToGrid(world));
                }

        foreach (var anchor in instance.GetComponentsInChildren<SocketAnchor>(true))
        {
            if (anchor.name == usedAnchorName)
                continue;

            Vector3 wc = SnapHalf(anchor.transform.position);

            frontier.Enqueue(new SocketRequest
            {
                worldCell = wc,
                normal = anchor.transform.forward,
                socketTag = anchor.socketTag,
                rotation = instance.transform.rotation
            });
        }

    }

    Vector3 RotateCell(Vector3 local, Quaternion rotation, Vector3 size)
    {
        Vector3 pivot = (size - Vector3.one) * 0.5f;
        Vector3 offset = local - pivot;
        return rotation * offset + pivot;


    }

    static SocketAnchor[] GetAnchors(ModuleDefinition def)
    {
        return def.prefab.GetComponentsInChildren<SocketAnchor>(true);
    }

    void GenerateInitialLayout()
    {
        Quaternion startRotation = Quaternion.identity;
        Vector3 startOrigin = Vector3.zero;

        GameObject instance = Instantiate(
            startModule.prefab,
            startOrigin,
            startRotation,
            transform
        );
        spawnedModules.Add(instance);

        for (int x = 0; x < startModule.size.x; x++)
            for (int y = 0; y < startModule.size.y; y++)
                for (int z = 0; z < startModule.size.z; z++)
                {
                    Vector3 local = new Vector3(x, y, z);
                    Vector3 world = startOrigin + RotateCell(local, startRotation, startModule.size);
                    occupied.Add(ToGrid(world));
                }

        foreach (var anchor in instance.GetComponentsInChildren<SocketAnchor>(true))
        {
            Vector3 wc = SnapHalf(anchor.transform.position);

            frontier.Enqueue(new SocketRequest
            {
                worldCell = wc,
                normal = anchor.transform.forward,
                socketTag = anchor.socketTag,
                rotation = instance.transform.rotation
            });
        }


        for (int i = 0; i < initialModules && frontier.Count > 0; i++)
        {
            var req = frontier.Dequeue();
            TryExpand(req);
        }
        navMeshSurface.BuildNavMesh();

    }

    bool Overlaps(Bounds candidate)
    {
        foreach (var b in placedBounds)
            if (b.Intersects(candidate))
                return true;
        return false;
    }

    Bounds MakeWorldBounds(ModuleDefinition def, Vector3 origin, Quaternion rot)
    {
        var extents = def.localBounds.extents;
        var center = def.localBounds.center;
        Vector3[] corners = new Vector3[8];
        int i = 0;
        for (int x = -1; x <= 1; x += 2)
            for (int y = -1; y <= 1; y += 2)
                for (int z = -1; z <= 1; z += 2)
                    corners[i++] = rot * (center + Vector3.Scale(extents, new Vector3(x, y, z)))
                                 + origin;

        Bounds b = new Bounds(corners[0], Vector3.zero);
        for (int j = 1; j < 8; j++) b.Encapsulate(corners[j]);
        return b;
    }


    Vector3 SnapHalf(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x * 2f) / 2f,
            Mathf.Round(v.y * 2f) / 2f,
            Mathf.Round(v.z * 2f) / 2f
        );
    }

}
