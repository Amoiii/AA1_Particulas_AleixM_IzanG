using UnityEngine;
using System.Collections.Generic;


public class SolarSystemSimulation : MonoBehaviour
{
    // Constante de gravitación 
    private const float G = 39.478f;

    [Header("Parámetros de la Simulación")]
    [Tooltip("Años simulados por cada frame. Cuanto más pequeño, más estable pero más lento.")]
    [SerializeField] private float timeStep = 0.0002f;

    [Tooltip("Factor para acelerar o frenar la simulación al vuelo.")]
    [SerializeField] private float timeScaleFactor = 1f;

    [Header("Escala de la escena")]
    [Tooltip("Multiplica la posición interna (en UA) para la posición en Unity.")]
    [SerializeField] private float sceneScale = 100f;

    [Header("Cuerpos en Orden: 0=Sol, 1=Mercurio, 2=Venus, 3=Tierra, 4=Luna, 5=Marte, 6=Júpiter, 7=Saturno, 8=Urano, 9=Neptuno")]
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
            2.45e-6f,
            3.0e-6f,
            3.7e-8f,
            3.23e-7f,
            9.55e-4f,
            2.86e-4f,
            4.36e-5f,
            5.15e-5f
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
            var b = new Body();
            b.name = bodyTransforms[i].name;


            b.mass = (i < masses.Length) ? masses[i] : 0f;


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
        float dt = timeStep * timeScaleFactor;
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
                    case 4: scaleVal = 4f; break;
                    case 5: scaleVal = 8f; break;
                    case 6: scaleVal = 40f; break;
                    case 7: scaleVal = 35f; break;
                    case 8: scaleVal = 25f; break;
                    case 9: scaleVal = 25f; break;
                }
                bodyTransforms[i].localScale = Vector3.one * scaleVal;
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            timeScaleFactor *= 2f;
        }


        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Underscore) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            timeScaleFactor /= 2f;
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            timeScaleFactor = 1f;
        }
    }
}
