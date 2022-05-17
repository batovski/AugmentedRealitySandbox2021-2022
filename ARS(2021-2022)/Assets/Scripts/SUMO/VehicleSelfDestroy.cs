using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSelfDestroy : MonoBehaviour
{
    public float TimeToDestroy = 15f;
    float timeleft = 0;
    Vector3 CurrentPos;
    Vector3 PrevPos;
    // Start is called before the first frame update
    void Start()
    {
        CurrentPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PrevPos = CurrentPos;
        CurrentPos = gameObject.transform.position;
        if(PrevPos == CurrentPos)
        {
            timeleft += Time.deltaTime;
        }
        else
        {
            timeleft = 0;
        }
        if (timeleft >= TimeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
