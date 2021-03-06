using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsController : MonoBehaviour
{
    public float CarSpeed = 10;
    Dictionary<string, GameObject> cars;

    // Start is called before the first frame update
    void Start()
    {
        cars = new Dictionary<string, GameObject>();
    }

    public void AddCar(string id, GameObject obj)
    {
        cars.Add(id, obj);
    }
    public void UpdateCarPos(string id, Vector3 pos, float angle = 0)
    {
        GameObject Cars_GO;
        if (cars.TryGetValue(id, out Cars_GO) && Cars_GO)
        {
            var newCarPos = new Vector3((float)pos.x, 0.0f, (float)pos.y);
            var rotation = Quaternion.Euler(0, angle, 0);
            Cars_GO.transform.rotation = rotation;
            Cars_GO.transform.position = Vector3.Lerp(Cars_GO.transform.position, newCarPos, Time.deltaTime * CarSpeed);
        }
    }

    public bool ContainsCar(string id)
    {
        GameObject value;
        return cars.TryGetValue(id, out value);
    }
    public void DestroyCar( string id)
    {
        cars.Remove(id);
        Destroy(cars[id]);
    }
}
