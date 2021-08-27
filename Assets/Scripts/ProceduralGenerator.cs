using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct MapObject
    {
        public GameObject prefab;
        public int amount;
        public float minHeight;
        public float maxHeight;
        public float maxAngle;
        public float minSize;
        public float maxSize;
    }

    public MapObject[] mapObjects;
    public float mapSize;

    void Start()
    {
        foreach (var obj in mapObjects)
        {
            for (int i = 0; i < obj.amount; i++)
            {
                Vector3 origin = new Vector3(Random.Range(-mapSize / 2f, mapSize / 2f), 500, Random.Range(-mapSize / 2f, mapSize / 2f));

                if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit))
                {
                    var instance = Instantiate(obj.prefab);
                    instance.transform.position = hit.point + Vector3.up * Random.Range(obj.minHeight, obj.maxHeight);
                    instance.transform.Rotate(new Vector3(Random.Range(0f, obj.maxAngle), Random.Range(0f, 360f), Random.Range(0f, obj.maxAngle)));
                    instance.transform.localScale = Vector3.one * Random.Range(obj.minSize, obj.maxSize);
                }
            }
        }
    }
}
