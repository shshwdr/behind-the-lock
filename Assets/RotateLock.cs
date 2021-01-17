using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLock : MonoBehaviour
{
    public float speed;
    public Transform target;
    bool isRotating;

    private Vector3 zAxis = new Vector3(0, 0, 1);

    private void Start()
    {

        //Utils.finishLevel += Rotate;
    }

    void Rotate()
    {
        isRotating = true;
        StartCoroutine(ExampleCoroutine());

    }
    IEnumerator ExampleCoroutine()
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        isRotating = false;
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            transform.RotateAround(target.position, zAxis, speed);
        }
    }
}
