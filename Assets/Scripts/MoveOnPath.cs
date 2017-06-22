using UnityEngine;
using System.Collections;

public class MoveOnPath : MonoBehaviour {

    public EditorPathScript myEditorPath;

    public int CurrentWayPointID = 0;
    public float speed = 2;
    private float reachDistance = .1f;
    public float rotationSpeed = 5.0f;
    public string pathName;

    private Vector3 lastPosition;
    private Vector3 currentPosition;

	// Use this for initialization
	void Start ()
    {
        myEditorPath = GameObject.Find(pathName).GetComponent<EditorPathScript>();
        lastPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float distance = Vector3.Distance(myEditorPath.pathObjects[CurrentWayPointID].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, myEditorPath.pathObjects[CurrentWayPointID].position, Time.deltaTime * speed);

        //look at direction of movement using paths and waypoints
        //useful maybe for something else
        //var rotation = Quaternion.LookRotation(myEditorPath.pathObjects[CurrentWayPointID].position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        if(distance <= reachDistance)
        {
            CurrentWayPointID++;
        }
        if(CurrentWayPointID >= myEditorPath.pathObjects.Count)
        {
            CurrentWayPointID = 0;
        }
	}
}
