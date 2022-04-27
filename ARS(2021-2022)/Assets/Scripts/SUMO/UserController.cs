﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public static UserController Instance;

    public GameObject Canvas;
    public float speed = 400.0f;
    public float ZoomSpeed = 500.0f;
    public float FOV = 60;

    public Camera Main_Camera;

    // Start is called before the first frame update
    void Awake()
    {
        //SINGELTON:
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        if (!Main_Camera.isActiveAndEnabled)
        {
            Main_Camera.gameObject.SetActive(true);
        }

        Main_Camera.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        Main_Camera.fieldOfView = FOV;
        Main_Camera.farClipPlane = 1000000.0f;
        Main_Camera.nearClipPlane = 0.001f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject TC = GameObject.Find("Traci_Controller");
            if (TC != null)
            {
                TC.GetComponent<TraciController>().OccupancyVisual = !TC.GetComponent<TraciController>().OccupancyVisual;
                TC.GetComponent<TraciController>().CarVisual = !TC.GetComponent<TraciController>().CarVisual;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            TraciController.Instance.BlockEntireRoad("-237758605#3_0");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (Canvas.gameObject.activeSelf)
            {
                Canvas.gameObject.SetActive(false);
            }
            else
            {
                Canvas.gameObject.SetActive(true);
            }
                
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Main_Camera.orthographic)
            {
                Main_Camera.orthographic = false;
            }
            else
            {
                Main_Camera.orthographic = true;
            }
            
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Main_Camera.transform.Translate(new Vector3(0.0f, speed * Time.deltaTime, 0.0f));
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Main_Camera.transform.Translate(new Vector3(0.0f, -speed * Time.deltaTime, 0.0f));
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Main_Camera.transform.Translate(new Vector3(-speed * Time.deltaTime, 0.0f, 0.0f));
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Main_Camera.transform.Translate(new Vector3(speed * Time.deltaTime, 0.0f, 0.0f));
        }

        if (Input.GetKey(KeyCode.PageDown) || Input.GetKey(KeyCode.Z))
        {
            Main_Camera.transform.Translate(new Vector3(0.0f, 0.0f, ZoomSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.PageUp) || Input.GetKey(KeyCode.X))
        {
            Main_Camera.transform.Translate(new Vector3(0.0f, 0.0f, -ZoomSpeed * Time.deltaTime));
        }

       /* if (Input.GetKey(KeyCode.Mouse1))
        {
            Main_Camera.transform.LookAt(Input.mousePosition);
        }*/
    }
}
