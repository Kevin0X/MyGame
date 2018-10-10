using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMap : MonoBehaviour
{
    [SerializeField]
    private float width = 10;
    [SerializeField]
    private float depth = 10;
    [SerializeField]
    private int maxHeight = 10;
    [SerializeField]
    private int minHeight = 3;
    [SerializeField]
    private bool isPerlinNoiseMap = true;
    [SerializeField]
    private float span = 0.2f;

    private float seedX, seedZ;
    private List<GameObject> MapObjs = new List<GameObject>();
    private List<GameObject> DesObjs = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            MapObjs.Add(Resources.Load<GameObject>("Prefabs/Map/PBOX_" + i.ToString()));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateMap();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }

    private void CreateMap()
    {
        foreach(GameObject go in DesObjs)
        {
            Destroy(go);
        }
        DesObjs.Clear();

        seedX = Random.value * 100f;
        seedZ = Random.value * 100f;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float y = SetY(x, z);
                if (y >= minHeight)
                {
                    int rand = Random.Range(0, 7);
                    GameObject go = Instantiate(MapObjs[rand], transform);
                    go.transform.position = new Vector3(x, 0, z);
                    DesObjs.Add(go);
                }
            }
        }
    }

    private float SetY(int x, int z)
    {
        float y = 0;
        if (isPerlinNoiseMap)
        {
            float xSample = (x + seedX) * span;
            float zSample = (z + seedZ) * span;
            float noise = Mathf.PerlinNoise(xSample, zSample);
            y = maxHeight * noise;
        }
        else
        {
            y = Random.Range(0, maxHeight);
        }
        y = Mathf.Round(y);
        return y;
    }
}
