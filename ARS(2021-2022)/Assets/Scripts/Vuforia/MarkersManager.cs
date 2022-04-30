using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkersManager : MonoBehaviour
{
    public float ARCameraHeight = 4350f;
    public float XDifference = -205;
    private Transform CameraTransform;
    [SerializeField]
    private Transform ARCameraTransform;

    void Start()
    {
        CameraTransform = UserController.Instance.gameObject.transform;
    }
    private Vector3 ConvertCordinatesToWorldPos(Vector3 position) 
    {
        float Z = (CameraTransform.position.y * position.y) / ARCameraHeight;
        float X = (CameraTransform.position.y * position.x) / ARCameraHeight;
        return new Vector3(X, 0, Z);
    }
    public Vector3 CreateProjection(Vector3 TrackerPos)
    {
       return (TrackerPos - ARCameraTransform.position).normalized;
    }
  
}