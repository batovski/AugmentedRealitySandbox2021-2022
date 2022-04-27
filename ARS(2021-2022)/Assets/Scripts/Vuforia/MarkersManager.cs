using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkersManager : MonoBehaviour
{
    public float ARCameraHeight = 4350f;
    private Transform CameraTransform;

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
        Vector3 Projection = CameraTransform.forward;
        Vector3 WorldPosition = ConvertCordinatesToWorldPos(TrackerPos);
        return new Vector3(Projection.x * WorldPosition.x, Projection.y * WorldPosition.y, Projection.z * WorldPosition.z);
    }
}