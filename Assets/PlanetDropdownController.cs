using UnityEngine;
using UnityEngine.UI;  // Required for Dropdown

public class PlanetDropdownController : MonoBehaviour
{
    public Dropdown planetDropdown; // Assign the dropdown in the Unity Inspector
    public GravityController gravityController; // Reference to your GravityController

    void Start()
    {
        // Ensure the dropdown and gravity controller are assigned
        if (planetDropdown != null && gravityController != null)
        {
            // Add listener to the dropdown to call SwitchPlanet when value changes
            planetDropdown.onValueChanged.AddListener(delegate {
                OnDropdownValueChanged(planetDropdown);
            });
        }
        else
        {
            Debug.LogError("Dropdown or GravityController is not assigned!");
        }
    }

    // This function is called whenever the dropdown value changes
    void OnDropdownValueChanged(Dropdown dropdown)
    {
        // Convert the dropdown value to the corresponding Planet enum
        GravityController.Planet selectedPlanet = (GravityController.Planet)dropdown.value;

        // Call the SwitchPlanet method in GravityController
        gravityController.SwitchPlanet(selectedPlanet);
    }
}