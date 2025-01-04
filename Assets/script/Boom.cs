using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    private void Destroying()
    {
        Destroy(gameObject);
    }
}
