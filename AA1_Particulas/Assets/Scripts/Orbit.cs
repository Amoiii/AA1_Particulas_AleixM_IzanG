using UnityEngine;
using System.Collections.Generic;

public class SolarSystemSimulation : MonoBehaviour
{
    // Constante de gravitación en (UA^3 / (año^2 * M_sun))
    private const float G = 39.478f;

    [SerializeField] private float timeStep = 0.0002f;

  
    [SerializeField] private float timeScaleFactor = 1f;

    
    [SerializeField] private float sceneScale = 100f;

    
    [SerializeField] private float sizeScale = 0.1f;

    
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
            1.0f,       // Sol
            1.66e-7f,   // Mercurio
            2.45e-6f,   // Venus
            3.0e-6f,    // Tierra
            3.7e-8f,    // Luna
            3.23e-7f,   // Marte
            9.55e-4f,   // Júpiter
            2.86e-4f,   // Saturno
            4.36e-5f,   // Urano
            5.15e-5f    // Neptuno
        };

        float[] distances = {
            0.0f,     
            0.39f,    
            0.72f,    
            1.00f,   
            1.0026f, 
            1.52f,   
            5.2f,     
            9.58f,   
            19.22f,   
            30.05f   
        };

        float[] speeds = {
            0.0f,  
            10.07f, 
            7.38f,  
            6.28f,  
            6.52f,  
            5.06f,  
            2.75f, 
            2.04f,  
            1.43f,  
            1.14f  
        };

        for (int i = 0; i < bodyTransforms.Length; i++)
        {
            Body b = new Body();
            b.name = bodyTransforms[i].name;
            b.mass = (i < masses.Length) ? masses[i] : 0f;

            float r = (i < distances.Length) ? distances[i] : 0f;
            b.position = new Vector3(r, 0f, 0f);

            float v = (i < speeds.Length) ? speeds[i] : 0f;
            b.velocity = (r > 0f) ? new Vector3(0f, 0f, v) : Vector3.zero;

          

           
           
            if (i == 10 || i == 11) // Lunas de Marte
            {
                if (bodies.Count > 5) // Marte
                {
                    Vector3 marsPos = bodies[5].position;
                    Vector3 marsVel = bodies[5].velocity;

                   
                    b.position = marsPos + new Vector3(0.05f + 0.05f * (i - 10), 0f, 0f);
                    b.velocity = marsVel + new Vector3(0f, 0.3f + 0.2f * (i - 10), 0f);
                }
            }
            else if (i == 12 || i == 13 || i == 14) // Lunas de Júpiter
            {
                if (bodies.Count > 6) // Júpiter
                {
                    Vector3 jupiterPos = bodies[6].position;
                    Vector3 jupiterVel = bodies[6].velocity;

                   
                    float offset = 0.1f + 0.03f * (i - 12);
                    float extraVel = 0.3f + 0.15f * (i - 12);

                    b.position = jupiterPos + new Vector3(offset, 0f, 0f);
                    b.velocity = jupiterVel + new Vector3(0f, extraVel, 0f);
                }
            }

            bodies.Add(b);
        }

        ApplyInitialTransforms();
    }

    void FixedUpdate()
    {
        float dt = timeStep * timeScaleFactor;  // años
        VelocityVerletStep(dt);

        for (int i = 0; i < bodies.Count; i++)
        {
            bodyTransforms[i].position = bodies[i].position * sceneScale;
        }
    }

    void VelocityVerletStep(float dt)
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
        Vector3[] acc = new Vector3[bodies.Count];

        for (int i = 0; i < bodies.Count; i++)
        {
            Vector3 a_i = Vector3.zero;
            for (int j = 0; j < bodies.Count; j++)
            {
                if (i == j) continue;

                Vector3 r_ij = bodies[j].position - bodies[i].position;
                float distSqr = r_ij.sqrMagnitude;
                float dist = Mathf.Sqrt(distSqr);

                if (dist > 1e-9f)
                {
                    float f = (G * bodies[j].mass) / distSqr;
                    a_i += f * r_ij.normalized;
                }
            }
            acc[i] = a_i;
        }
        return acc;
    }

    void ApplyInitialTransforms()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            bodyTransforms[i].position = bodies[i].position * sceneScale;
            if (i == 0)
            {
                
                bodyTransforms[0].localScale = new Vector3(70f, 70f, 70f);
            }
            else
            {
                float scaleVal = 0f;
                switch (i)
                {
                    case 1: scaleVal = 7f; break;  
                    case 2: scaleVal = 9f; break;  
                    case 3: scaleVal = 10f; break; 
                    case 4: scaleVal = 2f; break;  
                    case 5: scaleVal = 8f; break;  
                    case 6: scaleVal = 40f; break; 
                    case 7: scaleVal = 35f; break; 
                    case 8: scaleVal = 25f; break; 
                    case 9: scaleVal = 25f; break; 

                  
                    case 10: scaleVal = 1f; break;
                    case 11: scaleVal = 1f; break;

                   
                    case 12: scaleVal = 1.5f; break;
                    case 13: scaleVal = 1.5f; break;
                    case 14: scaleVal = 1.5f; break;

                    default: scaleVal = 1f; break;
                }
                bodyTransforms[i].localScale = Vector3.one * scaleVal * sizeScale;
            }
        }
    }

    void Update()
    {
        // Acelerar con +
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            timeScaleFactor *= 2f;
        }
        // Frenar con -
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Underscore) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            timeScaleFactor /= 2f;
        }
        // Restaurar velocidad por defecto (R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            timeScaleFactor = 1f;
        }
    }
}
