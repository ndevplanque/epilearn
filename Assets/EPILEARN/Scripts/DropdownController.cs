using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPDropdownController : MonoBehaviour
{
    public TMP_Dropdown planetDropdown; // Le composant Dropdown de TextMesh Pro
    public GravityController gravityController; // Référence à votre script GravityController

    private void Start()
    {
        // Assurez-vous que le Dropdown et le GravityController sont assignés
        if (planetDropdown == null || gravityController == null)
        {
            Debug.LogError("Le Dropdown ou GravityController n'est pas assigné dans l'inspecteur.");
            return;
        }

        // Remplissez le Dropdown avec les noms de planètes (assurez-vous que cela correspond à l'ordre de l'énum)
        planetDropdown.AddOptions(new List<string>
        {
            "Mercury", "Venus", "Earth", "Moon", "Mars",
            "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto"
        });

        // Fixer la valeur initiale pour correspondre à la planète sélectionnée
        planetDropdown.value = (int)gravityController.selectedPlanet;

        // Ajouter un écouteur pour détecter quand l'utilisateur sélectionne un nouvel élément
        planetDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int value)
    {
        // Convertir la valeur sélectionnée dans l'énumération Planet et changer les réglages de gravité
        gravityController.SwitchPlanet((GravityController.Planet)value);
    }
}