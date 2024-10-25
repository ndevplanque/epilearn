using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class ARTouchObject : MonoBehaviour
{
    private static readonly List<ARRaycastHit> hits = new();
    [SerializeField] private ObjectTouchedEvent m_OnTouch = new();

    // Référence au AR Raycast Manager
    private ARRaycastManager raycastManager;

    public ObjectTouchedEvent onTouch
    {
        get => m_OnTouch;
        set => m_OnTouch = value;
    }

    private void Awake()
    {
        // Récupère le AR Raycast Manager
        raycastManager = FindObjectOfType<ARRaycastManager>();
        if (raycastManager == null) Debug.LogError("ARTouchObject requires an ARRaycastManager in the scene.");
    }

    private void Update()
    {
        // Vérifie s'il y a un touch sur l'écran
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            // Vérifie si c'est le début du touch
            if (touch.phase == TouchPhase.Began)
            {
                // Effectue un raycast depuis le point de touch
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Vérifie d'abord si on touche un objet 3D
                if (Physics.Raycast(ray, out hit))
                    // Vérifie si l'objet touché est celui-ci
                    if (hit.collider.gameObject == gameObject)
                    {
                        HandleTouch();
                        return;
                    }

                // Si on n'a pas touché d'objet 3D, vérifie les surfaces AR
                if (raycastManager.Raycast(touch.position, hits))
                    // Vérifie si le point de touch est proche de cet objet
                    foreach (var arHit in hits)
                    {
                        var distance = Vector3.Distance(arHit.pose.position, transform.position);
                        if (distance < 0.1f) // Ajustez cette valeur selon vos besoins
                        {
                            HandleTouch();
                            return;
                        }
                    }
            }
        }
    }

    private void HandleTouch()
    {
        Debug.Log($"AR Touch detected on: {gameObject.name}");
        TriggerAction();
    }

    private void TriggerAction()
    {
        Debug.Log("Triggering OnTouch Events");
        m_OnTouch.Invoke();
    }

    [Serializable]
    public class ObjectTouchedEvent : UnityEvent
    {
    }
}