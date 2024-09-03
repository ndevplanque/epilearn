using UnityEngine;

public class GravityController : MonoBehaviour
{
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
    public Planet selectedPlanet = Planet.Earth;

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();
        // Initialize gravity based on selected planet
        SwitchPlanet(selectedPlanet);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Compute local gravity force based on object's mass
            Vector3 localGravity = Physics.gravity * rb.mass;

            // Apply the computed local gravity to the Rigidbody
            rb.AddForce(localGravity - Physics.gravity * rb.mass, ForceMode.Acceleration);
        }
        else
        {
            // Log an error if Rigidbody component is not found
            Debug.LogError("Rigidbody is not attached!");
        }
    }

    // Method to switch the planet and update gravity
    public void SwitchPlanet(Planet newPlanet)
    {
        selectedPlanet = newPlanet;
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

        // Adjust global gravity setting in Unity to match selected planet's gravity scale
        Physics.gravity = new Vector3(0.0f, -9.81f * gravityScale, 0.0f);
    }
}
