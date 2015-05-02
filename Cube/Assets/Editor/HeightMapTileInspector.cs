using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeightMapTile))]
public class HeightMapTileInspector : Editor {

	// Use this for initialization
	void Start () {
	
	}

    public override void OnInspectorGUI()
    {
        HeightMapTile myTarget = (HeightMapTile)target;

        if (myTarget.heights == null)
        {
            myTarget.heights = new float[4];
        }

        for (int i = 0; i < 4; i++)
        {
            myTarget.heights[i] = EditorGUILayout.FloatField("Height " + (i + 1), myTarget.heights[i]);
        }
        if (GUILayout.Button("Apply"))
        {
            myTarget.MakeHeightMesh();
        }
    }
}
