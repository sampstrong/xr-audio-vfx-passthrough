# XR Audio VFX 

A work in progress exploring relationships between visual effects and audio in mixed reality settings that incorporate hand tracking and physics-based interactions. The project has been created using the Oculus Integration Unity Package with passthrough on my Quest 2.

## Audio Reactive Explorations

- Scale
- Velocity
- Rotation
- Shader Parameters
    - Vertex displacement
    - Color changes
- VFX Parameters
    - Simulation speed
    - Color
    - Number of particles
    
## Physics Interactions

- The majority of the objects that make up the interactions have rigidbodies, and can be physically interacted with via hand tracking
- The walls of my room have also been mapped to allow for physics based interactions with the environment.

## Things to Note

- If you close this project, it will not run as is, as I've taken out the Oculus SDK, however the code I've written does not rely on it, and could easilty be transferred to a different project.
