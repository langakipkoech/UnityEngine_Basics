### Particle Force Calculation in Fluid Simulation

This code snippet is part of a fluid simulation based on SPH that calculates and handles forces applied to particles based on pressure and viscosity.

#### Key Variables:
- **`Vector3 pressureForce = Vector3.zero;`**
  - Initializes the `pressureForce` vector to `(0, 0, 0)`. This will later store the force exerted on a particle due to pressure interactions with nearby particles.
  
- **`Vector3 viscosityForce = Vector3.zero;`**
  - Initializes the `viscosityForce` vector to `(0, 0, 0)`. This will store the force due to the viscosity, simulating the resistance between particles moving past each other.

#### Purpose:
- These forces (pressure and viscosity) will be computed based on the distance and interactions between neighboring particles in the simulation. 
- Initially set to zero, these forces are updated as the simulation progresses, influencing particle movement and behavior in the fluid environment.
