using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanetSlider : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")] public GravityController gravityController;

    public Image planetImage;
    // public GameObject closeButton; // The X button
    public GameObject swipeIndicator;
    public GameObject panelContainer; // Container that will be shown/hidden

    [Header("Planet Settings")] public PlanetImageMapping[] planetMappings;
    private readonly float minDragDistance = 50f;

    private int currentIndex;
    private float dragStartX;

    private void Start()
    {
        // Sort mappings to match enum order
        Array.Sort(planetMappings, (a, b) => a.planet.CompareTo(b.planet));

        // Set initial planet based on GravityController's selectedPlanet
        if (gravityController != null) SwitchToPlanet(gravityController.selectedPlanet);

        // Hide panel by default
        ClosePanel();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartX = eventData.position.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Optional: Add visual feedback during drag
        swipeIndicator.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var dragDistance = eventData.position.x - dragStartX;

        if (Mathf.Abs(dragDistance) > minDragDistance)
        {
            if (dragDistance > 0)
            {
                // Swipe right - previous planet
                currentIndex--;
                if (currentIndex < 0)
                    currentIndex = planetMappings.Length - 1; // Boucler vers la fin
                UpdateDisplay();
            }
            else if (dragDistance < 0)
            {
                // Swipe left - next planet
                currentIndex++;
                if (currentIndex >= planetMappings.Length)
                    currentIndex = 0; // Boucler vers le début
                UpdateDisplay();
            }
        }
    }

    private void UpdateDisplay()
    {
        // Ajuster l'index pour boucler les images
        currentIndex = (currentIndex + planetMappings.Length) % planetMappings.Length;

        planetImage.sprite = planetMappings[currentIndex].planetSprite;

        if (gravityController != null) gravityController.SwitchPlanet(planetMappings[currentIndex].planet);
    }

    public void OpenPanel()
    {
        if (panelContainer != null)
        {
            panelContainer.SetActive(true);

            // Update to current planet in GravityController
            if (gravityController != null) SwitchToPlanet(gravityController.selectedPlanet);
        }
    }

    public void ClosePanel()
    {
        if (panelContainer != null) panelContainer.SetActive(false);
    }

    public void NextPlanet()
    {
        currentIndex++;
        if (currentIndex >= planetMappings.Length)
            currentIndex = 0; // Boucler vers le début
        UpdateDisplay();
    }

    public void PreviousPlanet()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = planetMappings.Length - 1; // Boucler vers la fin
        UpdateDisplay();
    }

    public void SwitchToPlanet(GravityController.Planet planet)
    {
        for (var i = 0; i < planetMappings.Length; i++)
            if (planetMappings[i].planet == planet)
            {
                currentIndex = i;
                UpdateDisplay();
                break;
            }
    }

    [Serializable]
    public struct PlanetImageMapping
    {
        public GravityController.Planet planet;
        public Sprite planetSprite;
    }
}