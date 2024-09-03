using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    public GameObject sun;
    public float sunRotationSpeed;

    [System.Serializable]
    public class Planet
    {
        public GameObject planetObject; // L'objet représentant la planète
        public float rotationSpeed; // Vitesse de rotation de la planète sur elle-même
        public float sunDistance; // Distance de la planète par rapport au Soleil
        public float sunOrbitSpeed; // Vitesse de l'orbite autour du Soleil
    }

    public List<Planet> planets = new List<Planet>(); // Liste des planètes

    private Vector3 orbitAxis;

    void Start()
    {
        orbitAxis = Vector3.up;

        foreach (Planet planet in planets)
        {
            // Positionner la planète à la bonne distance du Soleil
            planet.planetObject.transform.position = sun.transform.position + planet.planetObject.transform.right * planet.sunDistance * 1.5f;
        }
    }

    void Update()
    {
        foreach (Planet planet in planets)
        {
            // Orbite autour du Soleil
            planet.planetObject.transform.RotateAround(sun.transform.position, orbitAxis, planet.sunOrbitSpeed * Time.deltaTime / 2000);

            // Rotation sur elle-même
            planet.planetObject.transform.Rotate(Vector3.up, planet.rotationSpeed * Time.deltaTime / 2000);
        }
    }
}
