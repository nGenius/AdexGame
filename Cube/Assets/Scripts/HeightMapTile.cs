using UnityEngine;
using System.Collections;

public class HeightMapTile : MapTile
{
    public float[] heights;

    public void MakeHeightMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter)
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[24];
            vertices[0] = new Vector3(-0.5f, heights[0], 0.5f);
            vertices[1] = new Vector3(0.5f, heights[1], 0.5f);
            vertices[2] = new Vector3(0.5f, heights[2], -0.5f);
            vertices[3] = new Vector3(-0.5f, heights[3], -0.5f);

            vertices[4] = new Vector3(-0.5f, 0, 0.5f);
            vertices[5] = new Vector3(0.5f, 0, 0.5f);
            vertices[6] = new Vector3(0.5f, heights[1], 0.5f);
            vertices[7] = new Vector3(-0.5f, heights[0], 0.5f);

            vertices[8] = new Vector3(0.5f, 0, 0.5f);
            vertices[9] = new Vector3(0.5f, 0, -0.5f);
            vertices[10] = new Vector3(0.5f, heights[2], -0.5f);
            vertices[11] = new Vector3(0.5f, heights[1], 0.5f);

            vertices[12] = new Vector3(0.5f, 0, -0.5f);
            vertices[13] = new Vector3(-0.5f, 0, -0.5f);
            vertices[14] = new Vector3(-0.5f, heights[3], -0.5f);
            vertices[15] = new Vector3(0.5f, heights[2], -0.5f);

            vertices[16] = new Vector3(-0.5f, 0, -0.5f);
            vertices[17] = new Vector3(-0.5f, 0, 0.5f);
            vertices[18] = new Vector3(-0.5f, heights[0], 0.5f);
            vertices[19] = new Vector3(-0.5f, heights[3], -0.5f);
            
            vertices[20] = new Vector3(-0.5f, 0, -0.5f);
            vertices[21] = new Vector3(0.5f, 0, -0.5f);
            vertices[22] = new Vector3(0.5f, 0, 0.5f);
            vertices[23] = new Vector3(-0.5f, 0, 0.5f);

            Vector2[] uv = { 
                new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
                };


            int[] triangles = new int[] { 
                0, 1, 2, 0, 2, 3, 
                4, 5, 6, 4, 6, 7, 
                8, 9, 10, 8, 10, 11, 
                12, 13, 14, 12, 14, 15, 
                16, 17, 18, 16, 18, 19, 
                20, 21, 22, 20, 22, 23
            };


            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
            mesh.Optimize();

            meshFilter.sharedMesh = mesh;

            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider)
            {
                float maxHeight = GetMaxHeight();
                boxCollider.center = new Vector3(0, maxHeight/2.0f, 0);
                boxCollider.size = new Vector3(1, maxHeight, 1);
            }
        }
    }

    private float GetMaxHeight()
    {
        float maxValue = float.MinValue;
        for (int i = 0; i < heights.Length; i++)
        {
            if (maxValue < heights[i])
            {
                maxValue = heights[i];
            }
        }

        return maxValue;
    }
}
