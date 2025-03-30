using UnityEngine;
using System.Collections.Generic;
using System;

public class GravitySimulator : MonoBehaviour
{
    public GameObject sun;
    public List<GameObject> planets;
    public float gravitationalConstant = 1000f; // Adjust this value for scaling

    void FixedUpdate()
    {
        // Calculate gravitational forces between the Sun and each planet
        foreach (GameObject planetObj in planets)
        {
            Rigidbody planetRb = planetObj.GetComponent<Rigidbody>();
            Rigidbody sunRb = sun.GetComponent<Rigidbody>();
            Vector3 directionToSun = sunRb.position - planetRb.position;
            float distanceToSun = directionToSun.magnitude;
            Vector3 forceDirectionToSun = directionToSun.normalized;
            float forceMagnitudeToSun = gravitationalConstant * ((sunRb.mass * planetRb.mass) / (distanceToSun * distanceToSun));
            Vector3 forceToSun = forceDirectionToSun * forceMagnitudeToSun;
            planetRb.AddForce(forceToSun);
            // planet.currentVelocity += forceToSun / planet.mass * Time.fixedDeltaTime;
        }

        for (int i = 0; i < planets.Count; i++)
        {
            for (int j = i + 1; j < planets.Count; j++)
            {
                GameObject planetObjA = planets[i];
                GameObject planetObjB = planets[j];

                // Planet planetA = planetObjA.GetComponent<Planet>();
                // Planet planetB = planetObjB.GetComponent<Planet>();

                Rigidbody rbA = planetObjA.GetComponent<Rigidbody>();
                Rigidbody rbB = planetObjB.GetComponent<Rigidbody>();

                Vector3 directionAB = rbB.position - rbA.position;
                float distanceAB = directionAB.magnitude;
                if (distanceAB == 0)
                {
                    continue;
                }
                Vector3 forceDirectionAB = directionAB.normalized;
                float forceMagnitudeAB = gravitationalConstant * ((rbA.mass * rbB.mass) / (distanceAB * distanceAB));
                Vector3 forceAB = forceDirectionAB * forceMagnitudeAB;

                rbA.AddForce(forceAB);
                rbB.AddForce(-forceAB);

                // planetA.currentVelocity += forceAB / planetA.mass * Time.fixedDeltaTime;
                // planetB.currentVelocity -= forceAB / planetB.mass * Time.fixedDeltaTime;
            }
        }
    }
    void SetInitialVelocity()
    {
        List<GameObject> allCelestials = new List<GameObject>(planets);
        allCelestials.Add(sun);
        foreach (GameObject a in allCelestials)
        {
            foreach (GameObject b in allCelestials)
            {
                if (!a.Equals(b))
                {
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    a.transform.LookAt(b.transform);
     
                    a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((gravitationalConstant * m2) / r);
                }
            }
        }
    }

    private void Start()
    {
        SetInitialVelocity();
    }


    //// Update positions based on velocity for all planets
    //foreach (GameObject planetObj in planets)
    //{
    //    // Planet planet = planetObj.GetComponent<Planet>();
    //    Rigidbody planetRb = planetObj.GetComponent<Rigidbody>();

    //    planetRb.position += planet.currentVelocity * Time.fixedDeltaTime;
    //}
}