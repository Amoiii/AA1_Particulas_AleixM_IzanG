using UnityEngine;
using System.Collections.Generic;


public class SolarSystemSimulation : MonoBehaviour
{
    // Constante de gravitación en (UA^3 / (año^2 * M_sun))
    private const float G = 39.478f;

    
    [SerializeField] private float timeStep = 0.0002f;

   
    [SerializeField] private float timeScaleFactor = 1f;

   
    [SerializeField] private float sceneScale = 100f;

    
    public Transform[] bodyTransforms;


    private class Body
    {
        public string name;
        public float mass;         
        public Vector3 position;   
        public Vector3 velocity;   
    }

    
    private List<Body> bodies = new List<Body>();

    void Start()
    {

        float[] masses = {
            1.0f,          
            1.66e-7f,      
        
        };

        float[] distances = {
            0.0f,     
            0.39f,    
          
        };

        float[] speeds = {
            0.0f,
            10.07f,
            
        };


        for (int i = 0; i < bodyTransforms.Length; i++)
        {
            var b = new Body();
            b.name = bodyTransforms[i].name;

            // Masa
            b.mass = (i < masses.Length) ? masses[i] : 0f;

            // Posición inicial en UA sobre eje X
            float r = (i < distances.Length) ? distances[i] : 0f;
            b.position = new Vector3(r, 0f, 0f);


            float v = (i < speeds.Length) ? speeds[i] : 0f;
            b.velocity = (r > 0f) ? new Vector3(0f, 0f, v) : Vector3.zero;

            bodies.Add(b);
        }


        ApplyInitialTransforms();
    }

    void FixedUpdate()
    {
        float dt = timeStep * timeScaleFactor;  // en años
        Velocity(dt);


        for (int i = 0; i < bodies.Count; i++)
        {
            
            bodyTransforms[i].position = bodies[i].position * sceneScale;
        }
    }


    void Velocity(float dt)
    {
        
            Vector3[] accOld = ComputeAccelerations();

            for (int i = 0; i < bodies.Count; i++)
            {
                bodies[i].position += bodies[i].velocity * dt + 0.5f * accOld[i] * dt * dt;
            }

            Vector3[] accNew = ComputeAccelerations();

            for (int i = 0; i < bodies.Count; i++)
            {
                bodies[i].velocity += 0.5f * (accOld[i] + accNew[i]) * dt;
            }
        
    }

    Vector3[] ComputeAccelerations()
    {
       
    }


    void ApplyInitialTransforms()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
           
            bodyTransforms[i].position = bodies[i].position * sceneScale;

            // Ajusta la escala visual
            if (i == 0)
            {
                // Sol en 70
                bodyTransforms[0].localScale = new Vector3(70f, 70f, 70f);
            }
            else
            {
                float scaleVal = 0f;
                switch (i)
                {
                    case 1: scaleVal = 7f; break; // Mercurio
                   
                }
                bodyTransforms[i].localScale = Vector3.one * scaleVal;
            }
        }
    }

    
}
