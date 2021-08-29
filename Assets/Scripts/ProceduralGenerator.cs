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

    public GameObject player;

    public struct ObjectPlacement
    {
        public bool success;
        public GameObject instance;
        public RaycastHit hit;
    }

    public ObjectPlacement PlaceObject(GameObject prefab, bool onFloor, bool repeat)
    {
        Vector3 origin = new Vector3(Random.Range(-mapSize / 2f, mapSize / 2f), 500, Random.Range(-mapSize / 2f, mapSize / 2f));

        var j = 0;
        do
        {
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit) && (!onFloor || hit.collider.gameObject.tag == "Floor"))
            {
                var instance = Instantiate(prefab);

                return new ObjectPlacement()
                {
                    success = true,
                    hit = hit,
                    instance = instance
                };
            }

            j++;
        } while (repeat && j < 100);

        return new ObjectPlacement()
        {
            success = false
        };
    }

    void Start()
    {
        foreach (var obj in mapObjects)
        {
            for (int i = 0; i < obj.amount; i++)
            {
                var placement = PlaceObject(obj.prefab, true, obj.repeat);

                if (!placement.success) continue;
                var instance = placement.instance;
                var hit = placement.hit;

                instance.transform.position = hit.point + Vector3.up * Random.Range(obj.minHeight, obj.maxHeight);
                instance.transform.Rotate(new Vector3(Random.Range(0f, obj.maxAngle), Random.Range(0f, 360f), Random.Range(0f, obj.maxAngle)));
                instance.transform.localScale = Vector3.one * Random.Range(obj.minSize, obj.maxSize);
                instance.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_DarkColour", colours[Random.Range(0, colours.Length)]);
            }
        }

        {
            ObjectPlacement placement;

            do
            {
                placement = PlaceObject(player, true, true);
            } 
            while (!placement.success);

            placement.instance.transform.position = placement.hit.point + Vector3.up * 3f;
        }
        
    }

}
