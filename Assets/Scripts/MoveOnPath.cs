/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <MoveOnPath. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
following conditions are met:

	Redistributions of source code must retain the above copyright notice, this list of conditions and the
    following disclaimer.
    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
    following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUISNESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/


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
