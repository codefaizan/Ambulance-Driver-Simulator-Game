using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LookAtCamera());
    }

    IEnumerator LookAtCamera()
    {
        while (!GameController.levelCompleted)
        {
            if(Vector3.Distance(transform.position, Camera.main.transform.position) < 30f)
                transform.LookAt(Camera.main.transform);
            yield return new WaitForSeconds(1f);
        }
    }
}
