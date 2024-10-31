using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

//@author Nicolas Balh
public class ARTouchButton : MonoBehaviour
{
    [System.Serializable]
    public class ObjectClickedEvent : UnityEvent { }

    [Header("Events")]
    [SerializeField]
    private ObjectClickedEvent onObjectClicked = new ObjectClickedEvent();

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Vérifier pour les touches sur mobile
        if (Touchscreen.current != null)
        {
            // Vérifier s'il y a au moins un touch actif
            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                // Obtenir la position du touch
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                CheckRaycast(touchPosition);
            }
        }

        // Vérifier pour les clics de souris
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            CheckRaycast(mousePosition);
        }
    }

    void CheckRaycast(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            // Vérifier si l'objet touché est celui-ci
            if (hit.collider.gameObject == gameObject)
            {
                onObjectClicked.Invoke();
            }
        }
    }

    // Méthode publique pour ajouter des listeners par code si nécessaire
    public void AddClickListener(UnityAction listener)
    {
        onObjectClicked.AddListener(listener);
    }

    // Méthode publique pour retirer des listeners par code si nécessaire
    public void RemoveClickListener(UnityAction listener)
    {
        onObjectClicked.RemoveListener(listener);
    }
}