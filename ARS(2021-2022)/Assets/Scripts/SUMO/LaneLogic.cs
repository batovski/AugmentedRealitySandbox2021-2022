using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneLogic : MonoBehaviour
{
    Renderer laneRenderer;
    // Start is alled before the first frame update
    void Start()
    {
        laneRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    IEnumerator WaitAndUpdate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        laneRenderer.material = Resources.Load("Materials/Road_Material", typeof(Material)) as Material;
    }

    public void UpdateVisual(float o, Material Material1, Material Material2, Material Material3, Material Material4, Material Material5, Material Material6, Material Material7)
    {
        if (laneRenderer)
        {
            if (o >= 0.9f)
            {
                if (laneRenderer.material != Material7)
                    laneRenderer.material = Material7;
            }
            else if (o >= 0.75f)
            {
                if (laneRenderer.material != Material6)
                    laneRenderer.material = Material6;
            }
            else if (o >= 0.5f)
            {
                if (laneRenderer.material != Material5)
                    laneRenderer.material = Material5;
            }
            else if (o >= 0.1f)
            {
                if (laneRenderer.material != Material4)
                    laneRenderer.material = Material4;
            }
            else if (o >= 0.01f)
            {
                if (laneRenderer.material != Material3)
                    laneRenderer.material = Material3;
            }
            else if (o >= 0.001f)
            {
                if (laneRenderer.material != Material2)
                    laneRenderer.material = Material2;
            }
            else
            {
                if (laneRenderer.material != Material1)
                    laneRenderer.material = Material1;
            }
            StopAllCoroutines();
            StartCoroutine(WaitAndUpdate(1));
        }
    }
}
