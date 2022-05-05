using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRoadMarker : MonoBehaviour
{
    public MarkersManager MarkerManager;
    public float timeForTrigger; // (in seconds)
    public float xYTolerance, zTolerance;
    [Space]
    public Material BlockedRoadmaterial;
    public GameObject testObj;

    private float currentTimeOfTrigger = 0;
    bool eventTriggered = false;
    bool isTracked = false;

    LayerMask m_LayerMask;

    // Start is called before the first frame update
    void Start()
    {
        m_LayerMask = LayerMask.GetMask("Road");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTracked)
        {
            Vector3 HitDirection = MarkerManager.CreateProjection(gameObject.transform.position);
            LayerMask layerMask = LayerMask.GetMask("Ground");

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(UserController.Instance.Main_Camera.transform.position + new Vector3(-410 * UserController.Instance.Main_Camera.transform.position.y / 4350, 0, 0)
                , HitDirection, out hit, Mathf.Infinity, layerMask))
            {
                testObj.transform.position = hit.point;
            }
        }
        else
        {
            testObj.transform.position = Vector3.zero;
        }
        /*
        if (isTracked && !eventTriggered)
        {
            currentTimeOfTrigger += Time.deltaTime;
        }

        if(currentTimeOfTrigger >= timeForTrigger) 
        {
            eventTriggered = true;
            currentTimeOfTrigger = 0;
            //Call Some logic Here:
           // BlockRoads();
        }*/
    }
    public void StartTracking()
    {
        isTracked = true;

    }
    public void StopTracking()
    {
        isTracked = false;
        eventTriggered = false;
    }
    private void BlockRoads()
    {
        Vector3 HitDirection = MarkerManager.CreateProjection(gameObject.transform.position);
        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(UserController.Instance.Main_Camera.transform.position + new Vector3(-410 * UserController.Instance.Main_Camera.transform.position.y / 4350, 0, 0)
            , HitDirection, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(UserController.Instance.Main_Camera.transform.position + new Vector3(-410 * UserController.Instance.Main_Camera.transform.position.y/4350, 0, 0)
                , HitDirection * hit.distance, Color.yellow, 10,false);
            Debug.Log("Did Hit");
            Collider[] hitColliders = Physics.OverlapBox(hit.point, new Vector3(xYTolerance, xYTolerance, zTolerance), Quaternion.identity, m_LayerMask);
            Debug.Log(hitColliders.Length);
            //Check when there is a new collider coming into contact with the box
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                string ID = hitColliders[i].gameObject.name;
                TraciController.Instance.BlockEntireRoad(ID);
                LineRenderer RoadRenderer = hitColliders[i].gameObject.GetComponent<LineRenderer>();
                RoadRenderer.material = BlockedRoadmaterial;
            }
        }
    }
}
