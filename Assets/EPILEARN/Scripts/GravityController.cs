using UnityEngine;

public class GravityController : MonoBehaviour
{
    
    [Header("Audio Settings")]
    public AudioClip impactSound;      // Assign this in the Unity Inspector
    private AudioSource audioSource;
    private bool isFalling = false;


    
    // Enum to represent different planets with varying gravity  
    public enum Planet
    {
        Mercury,
        Venus,
        Earth, // Default  
        Moon,
        Mars,
        Jupiter,
        Saturn,
        Uranus,
        Neptune,
        Pluto
    }

    // The currently selected planet, default is Earth  
    [HideInInspector] 
    public Planet selectedPlanet = Planet.Earth;
    // Customize direction force
    public float initialDirectionEffectMultiplier;

    public bool debugMode = true;
    public float cameraOffset = 5f; // Distance from camera
    private Camera mainCamera;
    
    // Initial position to reset to
    private Vector3 initialPosition;

    private Rigidbody rb;
    private bool gravityOn = false; // Control variable for gravity

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject  
        rb = GetComponent<Rigidbody>();
        // Store the initial position  
        initialPosition = transform.position;
        // Initialize gravity based on selected planet  
        SetGravityState(gravityOn);
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (debugMode)
        {
            // Ensure the object is visible
            AdjustCameraToSeeObject();
            
            // Log initial positions
            Debug.Log($"GameObject Position: {transform.position}");
            Debug.Log($"Camera Position: {mainCamera.transform.position}");
        }
    }
    
    void AdjustCameraToSeeObject()
    {
        if (mainCamera != null)
        {
            // Position camera slightly above and behind the object
            Vector3 cameraPosition = transform.position - Vector3.forward * cameraOffset + Vector3.up * 2;
            mainCamera.transform.position = cameraPosition;
            mainCamera.transform.LookAt(transform.position);
        }
        else
        {
            Debug.LogError("No main camera found in the scene!");
        }
    }

    void FixedUpdate()
    {
        if (rb != null && gravityOn)
        {
            // Compute local gravity force based on object's mass  
            Vector3 localGravity = Physics.gravity * rb.mass;

            // Apply the computed local gravity to the Rigidbody  
            rb.AddForce(localGravity - Physics.gravity * rb.mass, ForceMode.Acceleration);
            if (!isFalling && rb.velocity.y < -0.1f)
            {
                isFalling = true;

            }
            // Stop falling sound if object is not falling anymore
            else if (isFalling && rb.velocity.y > -0.1f)
            {
                isFalling = false;
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
        else if (rb == null)
        {
            // Log an error if Rigidbody component is not found  
            Debug.LogError("Rigidbody is not attached!");
        }
    }

    // Method to switch the planet and update gravity scale  
    public void SwitchPlanet(Planet newPlanet)
    {
        selectedPlanet = newPlanet;

        if (gravityOn)
        {
            AdjustGravity();
        }
    }
    
    

    // Method to adjust the gravity settings based on the selected planet
    private void AdjustGravity()
    {
        float gravityScale = 1.0f;

        // Determine gravity scale based on selected planet  
        switch (selectedPlanet)
        {
            case Planet.Mercury:
                gravityScale = 0.377f;
                break;
            case Planet.Venus:
                gravityScale = 0.905f;
                break;
            case Planet.Earth:
                gravityScale = 1.0f;
                break;
            case Planet.Moon:
                gravityScale = 0.165f;
                break;
            case Planet.Mars:
                gravityScale = 0.378f;
                break;
            case Planet.Jupiter:
                gravityScale = 2.53f;
                break;
            case Planet.Saturn:
                gravityScale = 1.06f;
                break;
            case Planet.Uranus:
                gravityScale = 0.886f;
                break;
            case Planet.Neptune:
                gravityScale = 1.14f;
                break;
            case Planet.Pluto:
                gravityScale = 0.063f;
                break;
        }

        // Adjust global gravity setting in Unity to match the selected planet's gravity scale  
        Physics.gravity = new Vector3(0.0f, -9.81f * gravityScale, 0.0f);
    }

    
    void OnCollisionEnter(Collision collision)
    {
        float gravityScale = 1.0f;

        // Determine gravity scale based on the selected planet
        switch (selectedPlanet)
        {
            case Planet.Mercury:
                gravityScale = 0.377f;
                break;
            case Planet.Venus:
                gravityScale = 0.905f;
                break;
            case Planet.Earth:
                gravityScale = 1.0f;
                break;
            case Planet.Moon:
                gravityScale = 0.165f;
                break;
            case Planet.Mars:
                gravityScale = 0.378f;
                break;
            case Planet.Jupiter:
                gravityScale = 2.53f;
                break;
            case Planet.Saturn:
                gravityScale = 1.06f;
                break;
            case Planet.Uranus:
                gravityScale = 0.886f;
                break;
            case Planet.Neptune:
                gravityScale = 1.14f;
                break;
            case Planet.Pluto:
                gravityScale = 0.063f;
                break;
        }

        // Adjust the minimum collision speed based on the gravity of the planet
        float collisionThreshold = 1.0f * gravityScale;  // Scale threshold by gravity

        // Play impact sound if the collision speed is high enough
        if (collision.relativeVelocity.magnitude > collisionThreshold)
        {
            if (audioSource.isPlaying && audioSource.clip != impactSound)
            {
                audioSource.Stop();
            }

            if (impactSound != null)
            {
                audioSource.loop = false;
                audioSource.clip = impactSound;

                // Adjust volume based on the relative velocity to make it more natural
                audioSource.volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 10f);  // Scale between 0 and 1
                audioSource.Play();
            }

            isFalling = false;
        }
    }


    
    // Function to activate the gravity
    public void ActivateGravity()
    {
        if (gravityOn)
        {
            ResetPosition();
        }
        
        gravityOn = true;
        AdjustGravity();

        // Add an initial impulse to create slight horizontal movement
        if (rb != null)
        {
            // Create a force vector with a small impulse in a random direction along the X or Z axis
            rb.AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * initialDirectionEffectMultiplier, ForceMode.Impulse);
        }
    }

    // Function to deactivate the gravity
    public void DeactivateGravity()
    {
        gravityOn = false;
        Physics.gravity = Vector3.zero;  // Set global gravity to zero
    }

    // Function to toggle the gravity state
    public void SetGravityState(bool state)
    {
        gravityOn = state;

        if (gravityOn)
        {
            AdjustGravity();
        }
        else
        {
            Physics.gravity = Vector3.zero;
        }
    }

    // Function to reset position and pause gravity
    public void ResetPosition()
    {
        // Reset the object's position to the initial position
        transform.position = initialPosition;

        // Optionally reset the Rigidbody's velocity and angular velocity
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Deactivate gravity
        DeactivateGravity();
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        isFalling = false;
        
        if (debugMode)
        {
            AdjustCameraToSeeObject();
            Debug.Log($"Reset Position - GameObject at: {transform.position}");
        }
    }
}
