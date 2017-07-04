/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <ParticlePathFlow. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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
using System.Collections.Generic;
//using UnityStandardAssets.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
public class ParticlePathFlow : MonoBehaviour
{

    public class PathParticleTracker
    {
        public ParticleSystem.Particle particle;
        public float distance;
        public float rotation;

        public PathParticleTracker()
        {

            this.particle = new ParticleSystem.Particle();
            this.particle.remainingLifetime = 0.0f;

        }

        public void Revive(ParticleSystem systemRef)
        {

            this.distance = Random.Range(0.0f, 1.0f);
            this.rotation = Random.Range(0.0f, 360.0f);

            this.particle.startLifetime = systemRef.startLifetime;
            this.particle.remainingLifetime = this.particle.startLifetime;
            this.particle.color = systemRef.startColor;
            this.particle.size = systemRef.startSize;
            this.particle.rotation = systemRef.startRotation;
        }
    }

    public float emissionRate = 25.0f;
    private float emissionRateTracker = 0.0f;


    [Range(0.0f, 5.0f)]
    public float pathWidth = 0.0f;

    private int particle_count;
    private PathParticleTracker[] particle_trackerArray;
    private ParticleSystem.Particle[] particle_array;
    private ParticleSystem particle_system;


    private double editorTimeDelta = 0.0f;
    private double editorTimetracker = 0.0f;

    #region Standard assets variables
    // standard assets
    [SerializeField]
    private bool smoothRoute = true;

    public Transform[] Waypoints;

    private int points_num;
    private Vector3[] points_positions;
    private float[] points_distances;

    [HideInInspector]
    public float TotalDistance;

    public float editorVisualisationSubsteps = 100;

    //this being here will save GC allocs
    private int p0n;
    private int p1n;
    private int p2n;
    private int p3n;

    private float i;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;
    private Vector3 P3;

    #endregion


    void OnEnable()
    {

        Waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Waypoints[i] = transform.GetChild(i);
            Waypoints[i].gameObject.name = "point " + i;
        }
        points_num = Waypoints.Length;

        if (points_num > 1)
        {
            CachePositionsAndDistances();
        }

        particle_system = GetComponent<ParticleSystem>();
        particle_system.enableEmission = false;

        particle_array = new ParticleSystem.Particle[particle_system.maxParticles];

        particle_trackerArray = new PathParticleTracker[particle_system.maxParticles];
        for (int i = 0; i < particle_trackerArray.Length; i++)
            particle_trackerArray[i] = new PathParticleTracker();

        emissionRateTracker = 1.0f / emissionRate;


#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            editorTimetracker = EditorApplication.timeSinceStartup;
        }
#endif

    }

    private static Vector3 perpendicularDir = Vector3.up;
    void LateUpdate()
    {

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            editorTimeDelta = EditorApplication.timeSinceStartup - editorTimetracker;
            editorTimetracker = EditorApplication.timeSinceStartup;
        }
#endif

        if (transform.childCount <= 1)
            return;

        // emision
        if (emissionRateTracker <= 0.0f)
        {
            emissionRateTracker += 1.0f / emissionRate;

            RenewOneDeadParticle();
        }
        emissionRateTracker -= (Application.isPlaying ? Time.deltaTime : (float)editorTimeDelta);

        // age them
        foreach (PathParticleTracker tracker in particle_trackerArray)
            if (tracker.particle.remainingLifetime > 0.0f)
            {
                tracker.particle.remainingLifetime = Mathf.Max(tracker.particle.remainingLifetime - (Application.isPlaying ? Time.deltaTime : (float)editorTimeDelta), 0.0f);
            }


        float normLifetime = 0.0f;
        RoutePoint Rpoint;

        // move them
        foreach (PathParticleTracker tracker in particle_trackerArray)
            if (tracker.particle.remainingLifetime > 0.0f)
            {

                normLifetime = tracker.particle.remainingLifetime / tracker.particle.startLifetime;
                normLifetime = 1.0f - normLifetime;

                Rpoint = GetRoutePoint(normLifetime * TotalDistance);

                // 90 degree turn
                perpendicularDir.x = Rpoint.direction.y;
                perpendicularDir.y = -Rpoint.direction.x;
                perpendicularDir.z = Rpoint.direction.z;

                // rotate around Rpoint.direction
                perpendicularDir = Rotate_Direction(perpendicularDir, Rpoint.direction, tracker.rotation);

                // targetPos
                Rpoint.position += (pathWidth * tracker.distance) * perpendicularDir;

                tracker.particle.position = Rpoint.position;
                tracker.particle.velocity = Rpoint.direction;

            }

        particle_count = 0;

        // set the given array
        foreach (PathParticleTracker tracker in particle_trackerArray)
            if (tracker.particle.remainingLifetime > 0.0f)
            { // it's alive
                particle_array[particle_count] = tracker.particle;
                particle_count++;
            }

        particle_system.SetParticles(particle_array, particle_count);

    }

    void RenewOneDeadParticle()
    {

        for (int i = 0; i < particle_trackerArray.Length; i++)
            if (particle_trackerArray[i].particle.remainingLifetime <= 0.0f)
            {
                particle_trackerArray[i].Revive(particle_system);
                break;
            }
    }

    public static Vector3 Rotate_Direction(Vector3 dir, Vector3 axis, float angle)
    {

        return Quaternion.AngleAxis(angle, axis) * dir;
    }

    #region Standard assets section

    public RoutePoint GetRoutePoint(float dist)
    {
        // position and direction
        Vector3 p1 = GetRoutePosition(dist);
        Vector3 p2 = GetRoutePosition(dist + 0.1f);
        Vector3 delta = p2 - p1;
        return new RoutePoint(p1, delta.normalized);
    }


    public Vector3 GetRoutePosition(float dist)
    {

        dist = Mathf.Clamp(dist, 0.0f, TotalDistance);

        if (dist <= 0)
            return points_positions[0];
        else if (dist >= TotalDistance)
            return points_positions[points_num - 1];

        int point = 0;
        while (points_distances[point] < dist)
        {
            point++;
        }

        // get nearest two points, ensuring points wrap-around start & end of circuit
        p1n = point - 1;
        p2n = point;

        // found point numbers, now find interpolation value between the two middle points

        i = Mathf.InverseLerp(points_distances[p1n], points_distances[p2n], dist);

        if (smoothRoute)
        {
            p0n = Mathf.Clamp(point - 2, 0, points_num - 1);
            p3n = Mathf.Clamp(point + 1, 0, points_num - 1);


            P0 = points_positions[p0n];
            P1 = points_positions[p1n];
            P2 = points_positions[p2n];
            P3 = points_positions[p3n];

            return CatmullRom(P0, P1, P2, P3, i);
        }
        else
        {

            return Vector3.Lerp(points_positions[p1n], points_positions[p2n], i);
        }
    }


    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
    {
        // comments are no use here… it's the catmull-rom equation.
        // Un-magic this, lord vector!
        return 0.5f * ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i + (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
    }


    private void CachePositionsAndDistances()
    {
        // transfer the position of each point and distances between points to arrays for
        // speed of lookup at runtime
        points_positions = new Vector3[points_num];
        points_distances = new float[points_num];

        float accumulateDistance = 0;
        for (int i = 0; i < points_num - 1; ++i)
        {
            var t1 = Waypoints[i];
            var t2 = Waypoints[i + 1];
            if (t1 != null && t2 != null)
            {
                Vector3 p1 = t1.localPosition;
                Vector3 p2 = t2.localPosition;
                points_positions[i] = Waypoints[i].localPosition;
                points_distances[i] = accumulateDistance;
                accumulateDistance += (p1 - p2).magnitude;
            }
        }

        points_positions[points_num - 1] = Waypoints[points_num - 1].localPosition;
        points_distances[points_num - 1] = accumulateDistance;

        TotalDistance = accumulateDistance;
    }


    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }


    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }


    private void DrawGizmos(bool selected)
    {

        Waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Waypoints[i] = transform.GetChild(i);
            Waypoints[i].gameObject.name = "point " + i;
        }
        points_num = Waypoints.Length;

        if (points_num > 1)
        {
            CachePositionsAndDistances();
        }

        if (points_num <= 1)
            return;


        Gizmos.color = selected ? Color.yellow : new Color(1, 1, 0, 0.5f);
        Vector3 prev = Waypoints[0].position;
        if (smoothRoute)
        {
            for (float dist = 0; dist < TotalDistance; dist += TotalDistance / editorVisualisationSubsteps)
            {
                Vector3 next = transform.TransformPoint(GetRoutePosition(dist + 1));
                Gizmos.DrawLine(prev, next);
                prev = next;
            }

        }
        else
        {
            for (int n = 0; n < points_num - 1; ++n)
            {
                Vector3 next = Waypoints[n + 1].position;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }

    }


    public struct RoutePoint
    {
        public Vector3 position;
        public Vector3 direction;


        public RoutePoint(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }

    #endregion
}
