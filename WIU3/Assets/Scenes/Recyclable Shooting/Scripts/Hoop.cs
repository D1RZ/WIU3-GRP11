using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField] private List<GameObject> waypoints = new List<GameObject>();
    [SerializeField] private GameObject startingPoint;
    public RecyclableData.RecyclableType type;
    [SerializeField] private GameObject scoreTrigger;

    Vector3 destination;
    Vector3 direction;
    int index = 0;

    public float moveSpeed = 1.0f;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //set hoop position to starting point
        transform.position = startingPoint.transform.position;


        destination = waypoints[index].transform.position;
        direction = (destination - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(destination, transform.position);
        if (distance < 0.15f)
        {
            changeWaypoint();
        }
    }

    private void changeWaypoint()
    {
        index = (index + 1) % waypoints.Count;
        destination = waypoints[index].transform.position;
        direction = (destination - transform.position).normalized;
    }
}
