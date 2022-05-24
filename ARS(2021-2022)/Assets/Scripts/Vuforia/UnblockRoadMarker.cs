using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnblockRoadMarker : MonoBehaviour
{
    public MarkersManager MarkerManager;
    public float timeForTrigger; // (in seconds)
    public float xYTolerance, zTolerance;
    [Space]
    public Material UnblockedRoadmaterial;
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
    void Update()
    {
        if (isTracked)
        {
            Vector3 HitDirection = Quaternion.Euler(89.937f, 0, 0) * MarkerManager.CreateProjection(gameObject.transform.position);
            LayerMask layerMask = LayerMask.GetMask("Ground");

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(UserController.Instance.Main_Camera.transform.position + MarkerManager.GetImageShift()
                , HitDirection, out hit, Mathf.Infinity, layerMask))
            {
                testObj.transform.position = hit.point;
            }
        }

        if (isTracked && !eventTriggered)
        {
            currentTimeOfTrigger += Time.deltaTime;
        }

        if (currentTimeOfTrigger >= timeForTrigger)
        {
            eventTriggered = true;
            currentTimeOfTrigger = 0;
            //Call Some logic Here:
            UnblockRoads();
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
    private void UnblockRoads()
    {
        Vector3 HitDirection = Quaternion.Euler(89.937f, 0, 0) * MarkerManager.CreateProjection(gameObject.transform.position);
        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(UserController.Instance.Main_Camera.transform.position + MarkerManager.GetImageShift()
               , HitDirection, out hit, Mathf.Infinity, layerMask))
        {
            Collider[] hitColliders = Physics.OverlapBox(hit.point, new Vector3(xYTolerance, xYTolerance, zTolerance), Quaternion.identity, m_LayerMask);
            Debug.Log(hitColliders.Length);
            //Check when there is a new collider coming into contact with the box
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                string ID = hitColliders[i].gameObject.name;
                TraciController.Instance.UnBlockEntireRoad(ID);
                LineRenderer RoadRenderer = hitColliders[i].gameObject.GetComponent<LineRenderer>();
                RoadRenderer.material = UnblockedRoadmaterial;
            }
        }
    }
}
