using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class ExtensionMethods
{
    /// <summary>
    /// Floor each value in the Vector3 to an integer
    /// </summary>
    /// <param name="vector3">The Vector3 to floor</param>
    /// <returns>An array of 3 integers; X, Y, Z in their respective positions</returns>
    public static int[] FloorToInts(this Vector3 vector3)
    {
        return new int[3] {
            Mathf.FloorToInt(vector3.x),
            Mathf.FloorToInt(vector3.y),
            Mathf.FloorToInt(vector3.z)
        };
    }
}