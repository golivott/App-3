
using System;
using System.Collections;
using UnityEngine;

public static class Utils
{
    
    // Borrowed these from here: https://forum.unity.com/threads/tip-invoke-any-function-with-delay-also-with-parameters.978273/
    // Cause I didn't want to create a whole function just to change a bool after a set amount of time. These let me Invoke functions like this: () => { someBool = true } 
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }
 
    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}
