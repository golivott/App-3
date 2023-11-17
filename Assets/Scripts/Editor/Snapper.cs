using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Snapper
{
    private const string UNDO_STR_SNAP = "snap objects";

    [MenuItem("Edit/My Custom Tools/Snap Selected Object %&S", isValidateFunction: true)]
    public static bool SnapTheThingsValidate()
    {
        return Selection.gameObjects.Length > 0;
    }
    
    [MenuItem("Edit/My Custom Tools/Snap Selected Object %&S")]
    public static void SnapTheThings()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Undo.RecordObject( go.transform, UNDO_STR_SNAP);
            go.transform.position = go.transform.position.Round();
        }
    }
}
