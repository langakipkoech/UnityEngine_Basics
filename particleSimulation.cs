using UnityEngine;
using System.Collections.Generic;

public class FluidSimulation : MonoBehaviour
{
    public class Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public float density;
        public float pressure;

        public Particle(Vector3 position, Vector3 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            this.density = 0;
            this.pressure = 0;
        }
    }

    public List<Particle> particles = new List<Particle>();
    public GameObject particlePrefab; // Visual representation of the particle
    public int particlecount = 100;
    public float particleMass = 1f;
    public float smoothingRadius = 1f;
    public float stiffness = 200f;
    public float restDensity = 1000f;
    public float viscosity = 10f;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    public float deltaTime = 0.016f;

    private List<GameObject> particleVisuals = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < particlecount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-5f, 5f),
                Random.Range(0f, 5f),
                Random.Range(-5f, 5f)
            );

            particles.Add(new Particle(randomPosition, Vector3.zero));

            // Create visual representation
            GameObject particleVisual = Instantiate(particlePrefab, randomPosition, Quaternion.identity);
            particleVisuals.Add(particleVisual);
        }
    }

    void Update()
    {
        ComputeDensityAndPressure();
        ComputeForces();
        Integrate();
        VisualizeParticles();
    }

    void ComputeDensityAndPressure()
    {
        foreach (var particle in particles)
        {
            particle.density = 0f;

            foreach (var neighbour in particles)
            {
                float distance = Vector3.Distance(particle.position, neighbour.position);
                if (distance < smoothingRadius)
                {
                    float kernel = Mathf.Pow(1 - (distance / smoothingRadius), 3);
                    particle.density += particleMass * kernel;
                }
            }

            particle.pressure = stiffness * (particle.density - restDensity);
        }
    }

    void ComputeForces()
    {
        foreach (var particle in particles)
        {
            Vector3 pressureForce = Vector3.zero;
            Vector3 viscosityForce = Vector3.zero;

            foreach (var neighbour in particles)
            {
                float distance = Vector3.Distance(particle.position, neighbour.position);

                if (distance > 0 && distance < smoothingRadius)
                {
                    Vector3 direction = (particle.position - neighbour.position).normalized;
                    float pressureKernel = Mathf.Pow(1 - (distance / smoothingRadius), 3);
                    pressureForce += -direction * (particle.pressure + neighbour.pressure) / (2 * neighbour.density) * pressureKernel;

                    Vector3 velocityDifference = neighbour.velocity - particle.velocity;
                    float viscosityKernel = Mathf.Pow(1 - (distance / smoothingRadius), 3);
                    viscosityForce += viscosity * velocityDifference * viscosityKernel;
                }
            }

            Vector3 gravityForce = gravity * particleMass;

            particle.velocity += (pressureForce + viscosityForce + gravityForce) * deltaTime;
        }
    }

    void Integrate()
    {
        foreach (var particle in particles)
        {
            particle.position += particle.velocity * deltaTime;

            if (particle.position.y < 0)
            {
                particle.position.y = 0;
                particle.velocity.y *= -0.5f; // Simple damping
            }
        }
    }

    void VisualizeParticles()
    {
        for (int i = 0; i < particleVisuals.Count; i++)
        {
            particleVisuals[i].transform.position = particles[i].position;
        }
    }
}
