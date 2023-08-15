using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerPathRenderer : MonoBehaviour
{
    public Transform endPosition;
    [Tooltip("Get in runtime")]
    [SerializeField] Transform startPosition;
    [SerializeField] LineRenderer Path;
    [SerializeField] float pathHeightOffset = 1.25f;
    [SerializeField] float pathUpdateSpeed = 0.25f;
    bool isOnPath;
    public static PlayerPathRenderer i;

    Coroutine DrawPathCoroutine;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        startPosition = Ambulance.i.transform.Find("Line Renderer Pos");
        if (DrawPathCoroutine != null)
        {
            StopCoroutine(DrawPathCoroutine);
        }
        DrawPathCoroutine = StartCoroutine(DrawPathToTarget());
    }

    private IEnumerator DrawPathToTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(pathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        Path.startWidth = 2.5f;
        Path.endWidth = 2.5f;
        var lineTransform = Path.transform;
        lineTransform.localScale = Vector3.one;
        Path.transform.rotation = Quaternion.Euler(90, 0, 0);

        while (true)
        {
            int area = NavMesh.GetAreaFromName("Sidewalk");
            if (NavMesh.CalculatePath(startPosition.position, endPosition.position, area, path))
            {
                // calculate timer time based on navmesh path distance
                //if(!isOnPath)
                //{
                //    isOnPath = true;
                //    int timerSec = (int) (CalculateDistance(path)/1.5f);
                //    Timer.i.SetTimer(0, timerSec);
                //}
                Path.positionCount = path.corners.Length;
                
                for (int i = 0; i < path.corners.Length; i++)
                {
                    Path.SetPosition(i, path.corners[i] + Vector3.up * pathHeightOffset);
                }
            }
            else
            {
                print("Path not found");
            }
            yield return wait;
        }

    }

    float CalculateDistance(NavMeshPath path)
    {
        float distance = 0;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return distance;
    }
}
