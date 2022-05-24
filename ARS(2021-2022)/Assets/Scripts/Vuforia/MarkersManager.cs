using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkersManager : MonoBehaviour
{
    public float ARCameraHeight = 4350f;
    public float XDifference = 395;
    public float YDifference = -100;
    [SerializeField]
    private Transform ARCameraTransform;


    public Vector3 CreateProjection(Vector3 TrackerPos)
    {
       return (TrackerPos - ARCameraTransform.position).normalized;
    }
    
    public Vector3 GetImageShift()
    {
        return new Vector3(XDifference * UserController.Instance.gameObject.transform.position.y / ARCameraHeight,
            0 ,
            YDifference * UserController.Instance.gameObject.transform.position.y / ARCameraHeight);
    }
}