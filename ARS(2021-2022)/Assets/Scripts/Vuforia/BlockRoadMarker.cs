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
    void Update()
    {
        if (isTracked && !eventTriggered)
        {
            currentTimeOfTrigger += Time.deltaTime;
        }

        if(currentTimeOfTrigger >= timeForTrigger) 
        {
            eventTriggered = true;
            currentTimeOfTrigger = 0;
            //Call Some logic Here:
            BlockRoads();
        }
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
        Vector3 HitLocation = MarkerManager.CreateProjection(gameObject.transform.position);

        Collider[] hitColliders = Physics.OverlapBox(HitLocation, new Vector3(xYTolerance, xYTolerance, zTolerance), Quaternion.identity, m_LayerMask);
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
