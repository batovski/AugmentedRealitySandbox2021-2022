using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using Traci = CodingConnected.TraCI.NET;
using UnityEditor;

/// <summary>
/// A struct representing a Sumo Network Lane.
/// </summary>
[Serializable]
public struct Lane
{
    public string Id { get; set; }
    public string Index { get; set; }
    public string Speed { get; set; }
    public string Length { get; set; }
    public string Width { get; set; }
    public string Allow { get; set; }
    public string Disallow { get; set; }
    public string Shape { get; set; }
    public bool Built { get; set; }
    public string DefaultSpeed { get; set; }
    public bool ConstructionZone { get; set; }
}

/// <summary>
/// A struct representing a Sumo Network Edge.
/// </summary>
[System.Serializable]
public struct Road
{
    public string Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Name { get; set; }
    public string Shape { get; set; }
    public bool Built { get; set; }
    public string Type { get; set; }
    public string Function { get; set; }
    public List<Lane> Lanes { get; set; }
    public float Occupancy { get; set; }
}


/// <summary>
/// The Edge class stores Network Road and Lane information and builds roads (Edges) for SUMO networks.
/// </summary>
public class Edge : MonoBehaviour
{
    /// <summary>
    /// Handle to Edge Parent GameObject and script.
    /// </summary>    
    private GameObject Edges_GO;
    /// <summary>
    /// The list of the Networks roads.
    /// </summary>
    public List<Road> RoadList;
    public Shader Road_Shader;
    /// <summary>
    /// The width to make lanes in meters.
    /// </summary>
    public float LANEWIDTH = 3.4f;

    [SerializeField]
    private Material Material1;
    [SerializeField]
    private Material Material2;
    [SerializeField]
    private Material Material3;
    [SerializeField]
    private Material Material4;
    [SerializeField]
    private Material Material5;
    [SerializeField]
    private Material Material6;
    [SerializeField]
    private Material Material7;

    private List<Renderer> RoadsRenderer;

    /// <summary>
    /// Set the Edeg parent GameObject and create a new List<Road>() in Edge.RoadList.
    /// </summary>    
    void Start()
    {
        Edges_GO = GameObject.Find("Edges");
        RoadList = new List<Road>();
        Edges_GO.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        RoadsRenderer = new List<Renderer>();
    }


    /// <summary>
    /// Clear all saved Network Road Data.
    /// </summary>    
    public void ClearData()
    {
        RoadList.Clear();
    }

    /// <summary>
    /// Sumo shape sting to List of floats point order is
    /// x1, y1, x2, y2, ....
    /// <param name="shape">The string to parse the list from.</param>
    private List<float> ShapeStringToFloatList(string shape)
    {
        List<float> points = new List<float>();
        char[] find = new char[2];
        find[0] = ',';
        find[1] = ' ';
        string[] cuts = shape.Split(find);
        List<string> cutList = cuts.ToList();
        foreach (string cut in cutList)
        {
            points.Add(float.Parse(cut, CultureInfo.InvariantCulture.NumberFormat));
        }
        return points;
    }

    /// <summary>
    /// Builds Road shapes from lane and road position data.
    /// Shapes are built from Unitys LineRenderer.
    /// </summary>
    /// <param name="shapelist">A List of floating point x,y position</param>
    /// <param name="id">The Sumo ID as a string.</param>
    /// <param name="type">"Road" or "Pedestrian". This will set the materials used.</param>
    /// <param name="width">The road width as a float.</param>
    /// <param name="flat">True, use flat LineRenderer. False, use shader to extrude LineRenderer.</param>
    private void BuildShapeLR(List<Vector3> shapelist, string id, string type, float width, bool flat)
    {
        GameObject newShape = new GameObject();
        newShape.name = id;
        newShape.layer = LayerMask.NameToLayer("Road");

        LineRenderer LR = newShape.AddComponent<LineRenderer>();
        if (flat)
        {
            LR.material = Resources.Load("Materials/Road_Material", typeof(Material)) as Material;   
        }
        else
        {
            LR.material = new Material(Road_Shader);
        }
        LR.gameObject.layer = 6;
        LR.useWorldSpace = true;
        LR.textureMode = LineTextureMode.Tile;
        LR.alignment = LineAlignment.View;
        LR.endWidth = LR.startWidth = width;
        LR.numCapVertices = 5;
        LR.numCornerVertices = 5;
        LR.positionCount = shapelist.Count;
        LR.SetPositions(shapelist.ToArray());
        LR.transform.parent = Edges_GO.transform;
        RoadsRenderer.Add(LR);

        LineRenderer lineRenderer = LR;
        MeshCollider meshCollider = newShape.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
        newShape.AddComponent<MeshCollider>();
    }

    /// <summary>
    /// Builds Road shapes from lane and road position data.
    /// Shapes are built as polygon meshes.
    /// </summary>
    /// <param name="shapelist">A List of floating point x,y position</param>
    /// <param name="id">The string id of the Road or Lane Shape.</param>
    /// <param name="type">"Road" or "Pedestrian". This will set the materials used.</param>
    /// <param name="width">The Road or Lane width in meters.</param>
    private void BuildShapeMesh(List<Vector3> shapelist, string id, string type, float width)
    {
        GameObject chunk = new GameObject();
        chunk.name = id;

        MeshRenderer mr = chunk.AddComponent<MeshRenderer>();
        Material m;
        if (type == "Road")
        {
            //m = new Material(Road_Shader);
            m = Resources.Load("Materials/Road_Material", typeof(Material)) as Material;
        }
        else
        {
            m = Resources.Load("Materials/Road_Material", typeof(Material)) as Material;
        }
        
        mr.material = m;
        Mesh mesh = new Mesh();
        int numMeshVerts = shapelist.Count * 2;
        // Build Vertices
        float offset = LANEWIDTH / 2.0f;
        int slcounter = 0;
        Vector2[] verts = new Vector2[numMeshVerts];
        for (int i = 0; i < numMeshVerts; i+=2)
        {
            verts[i] = new Vector2(shapelist[slcounter].x - offset, shapelist[slcounter].z - offset);
            verts[i + 1] = new Vector2(shapelist[slcounter].x + offset, shapelist[slcounter].z + offset);
            slcounter++;   
        }

        Triangulator tr = new Triangulator(verts.ToArray());
        int[] indices = tr.Triangulate();

        Vector3[] vertices = new Vector3[numMeshVerts];
        for (int j = 0; j < numMeshVerts; j++)
        {
            vertices[j] = new Vector3(verts[j].x, 0.0f, verts[j].y);
        }

        Vector3[] normals = new Vector3[numMeshVerts];
        for (int k = 0; k < numMeshVerts; k++)
        {
            vertices[k] = -Vector3.up;
        }

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.normals = normals;
        mesh.RecalculateBounds();
        MeshFilter mf = chunk.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        chunk.transform.parent = Edges_GO.transform;
    }

    /// <summary>
    /// Parses the Road list and builds all valid Roads
    /// </summary>
    public void BuildEdges()
    {
        foreach(Road road in RoadList)
        {
           if(road.Function != "internal")
            {
                foreach (Lane lane in road.Lanes)
                {
                    if (lane.Shape != null)
                    {
                        string ltype;
                        List<float> ls = ShapeStringToFloatList(lane.Shape);
                        List<Vector3> lsv = new List<Vector3>();
                        for (int j = 0; j < ls.Count; j += 2)
                        {
                            lsv.Add(new Vector3(ls[j], 0.1f, ls[j + 1]));
                        }

                        if (lane.Disallow != null)
                        {
                            if (lane.Disallow.Contains("pedestrian"))
                            {
                                ltype = "Road";
                            }
                            else
                            {
                                ltype = "Other";
                            }
                        }
                        else
                        {
                            ltype = "Road";
                        }
                        BuildShapeLR(lsv, lane.Id, ltype, LANEWIDTH, true);
                        //BuildShapeMesh(lsv, lane.Id, ltype, LANEWIDTH);
                    }
                }
            }
        }
    }

    public void UpdateRoadsVisual( Traci.TraCIClient Client)
    {
        foreach (var child in RoadsRenderer)
        {
            float o = (float)Client.Lane.GetLastStepOccupancy(child.gameObject.name).Content;
            if (o >= 0.9f)
            {
                if(child.material != Material7)
                    child.material = Material7;
            }
            else if (o >= 0.8f)
            {
                if (child.material != Material6)
                    child.material = Material6;
            }
            else if (o >= 0.5f)
            {
                if (child.material != Material5)
                    child.material = Material5;
            }
            else if (o >= 0.1f)
            {
                if (child.material != Material4)
                    child.material = Material4;
            }
            else if (o >= 0.01f)
            {
                if (child.material != Material3)
                    child.material = Material3;
            }
            else if (o >= 0.001f)
            {
                if (child.material != Material2)
                    child.material = Material2;
            }
            else
            {
                if (child.material != Material1)
                    child.material = Material1;
            }
        }
    }
}
