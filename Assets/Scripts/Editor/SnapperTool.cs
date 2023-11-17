using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SnapperTool : EditorWindow
{
    public enum GridType
    {
        Cartesian,
        Polar,
        Triangle
    }
    
    const float TAU = 6.28318530718f;
    
    [MenuItem("Tools/Snapper")]
    public static void OpenTheThing() => GetWindow<SnapperTool>( "Snapper");
    
    public float gridSize = 1f;
    public GridType gridType = GridType.Cartesian;
    public int angularDivisions = 24;

    private SerializedObject so;
    private SerializedProperty propGridSize;
    private SerializedProperty propGridType;
    private SerializedProperty propAngularDivisions;
    
    private void OnEnable()
    {
        so = new SerializedObject(this);
        propGridSize = so.FindProperty("gridSize");
        propGridType = so.FindProperty("gridType");
        propAngularDivisions = so.FindProperty("angularDivisions");

        // load saved config
        gridSize = EditorPrefs.GetFloat("SNAPPER_TOOL_gridSize", 1f);
        gridType = (GridType)EditorPrefs.GetInt("SNAPPER_TOOL_gridType", 0);
        angularDivisions = EditorPrefs.GetInt("SNAPPER_TOOL_angularDivisions", 0);
        
        Selection.selectionChanged += Repaint;
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        // save config
        EditorPrefs.SetFloat("SNAPPER_TOOL_gridSize", gridSize);
        EditorPrefs.SetInt("SNAPPER_TOOL_gridType", (int)gridType);
        EditorPrefs.SetInt("SNAPPER_TOOL_angularDivisions", angularDivisions);
        
        Selection.selectionChanged -= Repaint;
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    void DuringSceneGUI( SceneView sceneView)
    {
        if ( Event.current.type == EventType.Repaint )
        {
            Handles.zTest = CompareFunction.LessEqual;
            const float gridDrawExtent = 16f;

            if (gridType == GridType.Cartesian)
                DrawCartesianGrid(gridDrawExtent);
            else if (gridType == GridType.Polar)
                DrawPolarGrid(gridDrawExtent);
            else
                DrawTriangleGrid(gridDrawExtent);
        }
    }

    void DrawTriangleGrid(float gridDrawExtent)
    {
        int lineCount = Mathf.RoundToInt((gridDrawExtent * 2) / gridSize);
        if (lineCount % 2 == 0)
            lineCount++; // make sure it's an odd number!
        int halfLineCount = lineCount / 2;
        
        for (int i = 0; i < lineCount; i++)
        {
            // Drawing verticals
            float intOffset = i - halfLineCount;
            float xCoord = intOffset * gridSize * 1.5f;
            float zCoord0 = halfLineCount * gridSize * 1.5f;
            float zCoord1 = -halfLineCount * gridSize * 1.5f;
            Vector3 p0 = new Vector3(xCoord, 0f, zCoord0);
            Vector3 p1 = new Vector3(xCoord, 0f, zCoord1);
            Handles.DrawAAPolyLine( p0, p1 );

            if (i % 2 == 0)
            {
                // Making diagonals
                Handles.DrawAAPolyLine(new Vector3(
                    -gridDrawExtent*2f*Mathf.Sin(1f/6f * TAU) + intOffset * gridSize * 1.5f,
                    0,
                    -gridDrawExtent*2f*Mathf.Cos(1f/6f * TAU)
                    ), new Vector3(
                        gridDrawExtent*2f*Mathf.Sin(1f/6f * TAU) + intOffset * gridSize * 1.5f,
                        0,
                        gridDrawExtent*2f*Mathf.Cos(1f/6f * TAU))
                    );
                Handles.DrawAAPolyLine(new Vector3(
                    -gridDrawExtent*2f*Mathf.Sin(2f/6f * TAU) + intOffset * gridSize * 1.5f,
                    0,
                    -gridDrawExtent*2f*Mathf.Cos(2f/6f * TAU)
                    ), new Vector3(
                        gridDrawExtent*2f*Mathf.Sin(2f/6f * TAU) + intOffset * gridSize * 1.5f ,
                        0,
                        gridDrawExtent*2f*Mathf.Cos(2f/6f * TAU))
                    );
            }
            
        }
        
        
    }

    void DrawPolarGrid(float gridDrawExtent)
    {
        int ringCount = Mathf.RoundToInt((gridDrawExtent) / gridSize);
        float radiusOuter = (ringCount - 1) * gridSize;

        // radial grid (rings)
        for (int i = 0; i < ringCount; i++)
        {
            Handles.DrawWireDisc( Vector3.zero, Vector3.up, i * gridSize );
        }
        
        // angular grid (lines)
        for (int i = 0; i < angularDivisions; i++)
        {
            float t = i / (float)angularDivisions;
            float angRad = t * TAU; // turns to radians
            float x = Mathf.Cos(angRad);
            float y = Mathf.Sin(angRad);
            Vector3 dir = new Vector3(x, 0f, y);
            Handles.DrawAAPolyLine( Vector3.zero, dir * radiusOuter);
        }
    }

    void DrawCartesianGrid( float gridDrawExtent)
    {
        int lineCount = Mathf.RoundToInt((gridDrawExtent * 2) / gridSize);
        if (lineCount % 2 == 0)
            lineCount++; // make sure it's an odd number!
        int halfLineCount = lineCount / 2;

        for (int i = 0; i < lineCount; i++)
        {
            float intOffset = i - halfLineCount;
            float xCoord = intOffset * gridSize;
            float zCoord0 = halfLineCount * gridSize;
            float zCoord1 = -halfLineCount * gridSize;
            Vector3 p0 = new Vector3(xCoord, 0f, zCoord0);
            Vector3 p1 = new Vector3(xCoord, 0f, zCoord1);
            Handles.DrawAAPolyLine( p0, p1 );

            p0 = new Vector3(zCoord0, 0f, xCoord);
            p1 = new Vector3(zCoord1, 0f, xCoord);
            Handles.DrawAAPolyLine( p0, p1 );
        }
    }

    private void OnGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(propGridType);
        EditorGUILayout.PropertyField(propGridSize);
        if (gridType == GridType.Polar)
        {
            EditorGUILayout.PropertyField(propAngularDivisions);
            propAngularDivisions.intValue = Mathf.Max(4, propAngularDivisions.intValue);
        }
        so.ApplyModifiedProperties();
        
        using (new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0))
        {
            if (GUILayout.Button("Snap Selection"))
                SnapSelection();
        }
    }

    void SnapSelection()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Undo.RecordObject( go.transform, "snap objects");
            go.transform.position =
                GetSnappedPosition(go.transform.position);  // go.transform.position.Round( gridSize );
        }
    }

    Vector3 GetSnappedPosition(Vector3 posOriginal)
    {
        if (gridType == GridType.Cartesian)
            return posOriginal.Round(gridSize);

        if (gridType == GridType.Polar)
        {
            Vector2 vec = new Vector2(posOriginal.x, posOriginal.z);
            float dist = vec.magnitude;
            float distSnapped = dist.Round(gridSize);

            float angRad = Mathf.Atan2(vec.y, vec.x);
            float angTurns = angRad / TAU;
            float angTurnsSnapped = angTurns.Round(1f / angularDivisions);
            float angRadSnapped = angTurnsSnapped * TAU;

            Vector2 snappedDir = new Vector2(Mathf.Cos(angRadSnapped), Mathf.Sin(angRadSnapped));
            Vector2 snappedVec = snappedDir * distSnapped;

            return new Vector3(snappedVec.x, posOriginal.y, snappedVec.y);
        }

        if (gridType == GridType.Triangle)
        {
            // This is actually for snapping to a hexagon grid. See https://www.redblobgames.com/grids/hexagons/
            // Convert to hexagon cube coord
            float q = (2f / 3f * posOriginal.x) / gridSize;
            float r = (-1f / 3f * posOriginal.x + Mathf.Sqrt(3f) / 3f * posOriginal.z) / gridSize;
            float s = -q - r;

            // Rounding
            var qRound = Mathf.Round(q);
            var rRound = Mathf.Round(r);
            var sRound = Mathf.Round(s);
            var qDiff = Mathf.Abs(qRound - q);
            var rDiff = Mathf.Abs(rRound - r);
            var sDiff = Mathf.Abs(sRound - s);
            if (qDiff > rDiff && qDiff > sDiff)
            {
                qRound = -rRound - sRound;
            }
            else if (rDiff > sDiff)
            {
                rRound = -qRound - sRound;
            }
            
            // Convert to pixel coord
            var x = gridSize * (3f / 2f * qRound);
            var y = gridSize * (Mathf.Sqrt(3f) / 2f * qRound + Mathf.Sqrt(3f) * rRound);
            return new Vector3(x, posOriginal.y, y);
        }

        return default;
    }
}
