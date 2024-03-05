using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public event Action<Vector3[]> OnNewPointsDown;

    public Material material;
    public Transform leftPt;
    public Transform rightPt;

    public Vector3 BottomRightPoint { get => rightPt.position; }
    public Vector3 BottomLeftPoint { get => leftPt.position; }
    public Vector3 TopRightPoint { get => rightPt.position + (rightPt.up * height); }
    public Vector3 TopLeftPoint { get => leftPt.position + (leftPt.up * height); }

    public float spawnDelay;
    private float elapsedTime;
    public float height;

    public bool drawTrail = true;
    public void SetDrawTrail(bool drawTrail) { this.drawTrail = drawTrail; }

    private bool firstTime = true;
    //private bool isEven;

    GameObject line;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    List<Vector3> verticesDef;
    List<int> trianglesDef;

    int numTriangles = 0;

    // Update is called once per frame
    void Update()
    {
        if(elapsedTime >= spawnDelay && drawTrail)
        {
            elapsedTime = 0f;
            CreateTrail();
        }
        
        elapsedTime += Time.deltaTime;
    }

    private void CreateTrail()
    {
        Vector3[] vertices = null;
        int[] triangles = null;
        
        if (firstTime)
        {
            vertices = new Vector3[] {
                                    rightPt.position, 
                                    leftPt.position,
                                    rightPt.position + (rightPt.up * height),
                                    leftPt.position + (leftPt.up * height) };

            triangles = new int[] {
                                0, 1, 3, //front
                                0, 3, 2 };

            line = new GameObject();
            line.name = "Trail";
            line.tag = "Trail";
            line.layer = 7;

            meshFilter = line.AddComponent<MeshFilter>();
            line.AddComponent<MeshRenderer>().material = material;

            meshCollider = line.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.mesh;

            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.triangles = triangles;

            verticesDef = new List<Vector3>();
            trianglesDef = new List<int>();

            foreach (Vector3 v in vertices)
                verticesDef.Add(v);

            foreach (int t in triangles)
                trianglesDef.Add(t);

            numTriangles = 4;
            firstTime = false;
            //isEven = false;
            return;
        }
        
        if (/*isEven*/true)
        {
            verticesDef.Add(rightPt.position);
            verticesDef.Add(leftPt.position);
            verticesDef.Add(rightPt.position + (rightPt.up * height));
            verticesDef.Add(leftPt.position + (leftPt.up * height));

            //left normal side 
            trianglesDef.Add(numTriangles + 1);
            trianglesDef.Add(numTriangles + 3);
            trianglesDef.Add(numTriangles - 1);

            trianglesDef.Add(numTriangles + 1);
            trianglesDef.Add(numTriangles - 1);
            trianglesDef.Add(numTriangles - 3);

            //left back side 
            trianglesDef.Add(numTriangles + 1);
            trianglesDef.Add(numTriangles - 1);
            trianglesDef.Add(numTriangles + 3);

            trianglesDef.Add(numTriangles + 1);
            trianglesDef.Add(numTriangles - 3);
            trianglesDef.Add(numTriangles - 1);

            //top normal side 
            trianglesDef.Add(numTriangles - 2);
            trianglesDef.Add(numTriangles + 3);
            trianglesDef.Add(numTriangles + 2);

            trianglesDef.Add(numTriangles - 2);
            trianglesDef.Add(numTriangles - 1);
            trianglesDef.Add(numTriangles + 3);

            //top back side 
            trianglesDef.Add(numTriangles - 2);
            trianglesDef.Add(numTriangles + 2);
            trianglesDef.Add(numTriangles + 3);

            trianglesDef.Add(numTriangles - 2);
            trianglesDef.Add(numTriangles + 3);
            trianglesDef.Add(numTriangles - 1);

            //right normal side 
            trianglesDef.Add(numTriangles - 4);
            trianglesDef.Add(numTriangles + 2);
            trianglesDef.Add(numTriangles);

            trianglesDef.Add(numTriangles - 4);
            trianglesDef.Add(numTriangles - 2);
            trianglesDef.Add(numTriangles + 2);

            //right back side 
            trianglesDef.Add(numTriangles - 4);
            trianglesDef.Add(numTriangles);
            trianglesDef.Add(numTriangles + 2);

            trianglesDef.Add(numTriangles - 4);
            trianglesDef.Add(numTriangles + 2);
            trianglesDef.Add(numTriangles - 2);
        }

        Vector3[] newPoints = new Vector3[]
        {
            verticesDef[numTriangles],     // Right bottom point 
            verticesDef[numTriangles + 1], // Left bottom point 
            verticesDef[numTriangles + 2], // Right top point 
            verticesDef[numTriangles + 3], // Left top point 
        };
        OnNewPointsDown?.Invoke(newPoints);

        numTriangles += 4;

        meshFilter.mesh.vertices = verticesDef.ToArray();
        meshFilter.mesh.triangles = trianglesDef.ToArray();

        meshCollider.sharedMesh = meshFilter.mesh;
    }
}
