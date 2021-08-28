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
        public bool repeat;
    }

    public MapObject[] mapObjects;
    public Color[] colours;
    public float mapSize;

    void Start()
    {
        foreach (var obj in mapObjects)
        {
            for (int i = 0; i < obj.amount; i++)
            {
                Vector3 origin = new Vector3(Random.Range(-mapSize / 2f, mapSize / 2f), 500, Random.Range(-mapSize / 2f, mapSize / 2f));

                var valid = false;
                var j = 0;
                do {
                    if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit) && hit.collider.gameObject.tag == "Floor")
                    {
                        var instance = Instantiate(obj.prefab);
                        instance.transform.position = hit.point + Vector3.up * Random.Range(obj.minHeight, obj.maxHeight);
                        instance.transform.Rotate(new Vector3(Random.Range(0f, obj.maxAngle), Random.Range(0f, 360f), Random.Range(0f, obj.maxAngle)));
                        instance.transform.localScale = Vector3.one * Random.Range(obj.minSize, obj.maxSize);
                        valid = true;
                        instance.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_DarkColour", colours[Random.Range(0, colours.Length)]);
                    }

                    j++;
                } while (obj.repeat && !valid && j < 100);
            }
        }
    }
}
