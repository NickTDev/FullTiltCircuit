using UnityEngine;

[RequireComponent(typeof(PlayerTrail))]
public class PlayerTrailConnector : MonoBehaviour
{
    private PlayerTrail playerTrail;

    GameObject connector;
    MeshFilter meshFilter;

    Vector3[] points;

    private void Awake()
    {
        playerTrail = GetComponent<PlayerTrail>();
        points = new Vector3[8];
    }

    private void OnEnable()
    {
        playerTrail.OnNewPointsDown += OnNewPointsDown;
    }

    private void OnDisable()
    {
        playerTrail.OnNewPointsDown -= OnNewPointsDown;
    }

    private void Start()
    {
        // Create connector object 
        connector = new GameObject();
        connector.name = "TrailConnector";
        connector.tag = "Trail";
        connector.layer = 7;

        // Add Components 
        meshFilter = connector.AddComponent<MeshFilter>();
        connector.AddComponent<MeshRenderer>().material = playerTrail.material;

        /* Vertex Map (Larger Center part is the top) 
         *  7 - 3 --- 2 - 5   <- These are closer to the player 
         *  | \ | \   | \ |
         *  |  \|   \ |  \|
         *  6 - 1 --- 0 - 4   <- These are closer to the trail 
         */

        // Set up the triangles (the verts will be later in update) 
        int[] triangles = new int[]
        {
            // Left front face 
            1, 7, 3,
            1, 6, 7,

            // Left back face 
            1, 3, 7,
            1, 7, 6,

            // Top front face 
            0, 3, 1,
            0, 2, 3,

            // Top back face 
            0, 1, 3,
            0, 3, 2,

            // Right front face 
            4, 2, 5,
            4, 0, 2,

            // Right back face 
            4, 5, 2,
            4, 2, 0
        };

        meshFilter.mesh.vertices = points;
        meshFilter.mesh.triangles = triangles;
    }

    private void LateUpdate()
    {
        if (!playerTrail.drawTrail)
            return;

        // Update the points to be with the player 
        points[5] = playerTrail.BottomRightPoint;
        points[7] = playerTrail.BottomLeftPoint;
        points[2] = playerTrail.TopRightPoint;
        points[3] = playerTrail.TopLeftPoint;

        // Update the filter 
        meshFilter.mesh.vertices = points;
        meshFilter.mesh.RecalculateNormals();
        meshFilter.mesh.RecalculateBounds();
    }

    private void OnNewPointsDown(Vector3[] newPoints)
    {
        // Update the points to the most recent end of the trail 
        points[4] = newPoints[0]; // Right bottom point 
        points[6] = newPoints[1]; // Left bottom point 
        points[0] = newPoints[2]; // Right top point 
        points[1] = newPoints[3]; // Left top point 
    }
}
