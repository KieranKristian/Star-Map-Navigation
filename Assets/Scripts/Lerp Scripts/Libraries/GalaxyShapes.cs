using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GalaxyShapes 
{
    //Galaxy Shape Calculations
    /// <summary>
    /// Returns a random position within a concave sphere
    /// </summary>
    public static Vector3 RandomDistanceConcave(int radius) {
        float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2) * radius;
        float v = UnityEngine.Random.value;
        float phi = Mathf.Acos((2 * v) - 1) * radius;
        float r = Mathf.Pow(UnityEngine.Random.value, 1 / 3) * radius;

        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);
        float sinPhi = Mathf.Sin(phi);
        float cosPhi = Mathf.Cos(phi);

        float x = r * sinPhi * cosTheta;
        float y = r * cosPhi * sinPhi;
        float z = r * sinPhi * sinTheta;

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Returns a random location in a double helix shape
    /// </summary>
    public static Vector3 RandomDistanceHelix(int radius, Transform transform) {
        float r = radius * Mathf.Sqrt(UnityEngine.Random.value);
        float theta = UnityEngine.Random.value * 2 * Mathf.PI;

        float x = transform.position.x + r * Mathf.Cos(theta);
        float y = transform.position.y + r * Mathf.Tan(theta);
        float z = transform.position.z + r * Mathf.Sin(theta);

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Returns a random location in a flat disc shape
    /// </summary>
    public static Vector3 RandomDistanceDisc(int radius, Transform transform) {
        float r = radius * Mathf.Sqrt(UnityEngine.Random.value);
        float theta = UnityEngine.Random.value * 2 * Mathf.PI;

        float x = transform.position.x + r * Mathf.Cos(theta);
        float z = transform.position.z + r * Mathf.Sin(theta);

        return new Vector3(x, 0, z);
    }

    /// <summary>
    /// Returns a random location in a bowl shape
    /// </summary>
    public static Vector3 RandomDistanceBowl(int radius) {
        float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2) * radius;
        float v = UnityEngine.Random.value;
        float phi = Mathf.Acos((2 * v) - 1) * radius;
        float r = Mathf.Pow(UnityEngine.Random.value, 1 / 3) * radius;

        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);
        float sinPhi = Mathf.Sin(phi);
        float cosPhi = Mathf.Sin(phi);

        float x = r * sinPhi * cosTheta;
        float y = r * cosPhi * sinPhi;
        float z = r * sinPhi * sinTheta;

        return new Vector3(x, y - 75, z);
    }

    /// <summary>
    /// Returns a random location in a hollow sphere shape
    /// </summary>
    public static Vector3 RandomDistanceHollowSphere(int radius) {
        float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2) * radius;
        float v = UnityEngine.Random.value;
        float phi = Mathf.Acos((2 * v) - 1) * radius;
        float r = Mathf.Pow(UnityEngine.Random.value, 1 / 3) * radius;

        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);
        float sinPhi = Mathf.Sin(phi);
        float cosPhi = Mathf.Cos(phi);

        float x = r * sinPhi * cosTheta;
        float y = r * sinPhi * sinTheta;
        float z = r * cosPhi;

        return new Vector3(x, y, z);
    }
}
