using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour 
{
    public List<Transform> Waypoints;
    public float MoveSpeed;

    private int currentIndex;
    private int targetIndex;

    private void Start()
    {
        StartCoroutine(MoveThroughWaypoints());
    }

    private void Update()
    {
  
    }

    private IEnumerator MoveThroughWaypoints()
    {
        if (Waypoints == null || Waypoints.Count < 2)
        {
            yield break;
        }

        int index = 0;
        int direction = 1;

        while (true)
        {
            Vector3 target = Waypoints[index].position;

            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                // transform.Translate(transform.position + (target - transform.position).normalized * MoveSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, target, MoveSpeed * Time.deltaTime);
                yield return null;
            }

            if (index == Waypoints.Count - 1)
            {
                direction = -1;
            }
            else if (index == 0)
            {
                direction = 1;
            }

            index += direction; // index = index + direction;
        }
    }
}