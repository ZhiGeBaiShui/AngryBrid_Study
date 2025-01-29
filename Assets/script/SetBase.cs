using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class SetBase : MonoBehaviour
{
    public List<Transform> grounds;
    public Vector3 startPosition = new (0, 0, 0);
    public Vector3 gapVector = new (3.4f, 0, 0);
    public void UpdatePositions()
    {
        Vector3 startNow = startPosition;
        foreach(Transform groundNow in grounds)
        {
            // Debug.Log(startNow);
            groundNow.localPosition = startNow;
            // Debug.Log(groundNow.position);
            startNow += gapVector;
        }
    }
}
