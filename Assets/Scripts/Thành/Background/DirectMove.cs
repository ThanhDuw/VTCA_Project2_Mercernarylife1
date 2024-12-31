using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectMove : MonoBehaviour
{
    [Tooltip("Moving speed on X axis in local space")]
    public float speed;

    //moving the object with the defined speed
    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
