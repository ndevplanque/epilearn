using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

/// <summary>
/// Handles dismissing the object menu when clicking out the UI bounds, and showing the
/// menu again when the create menu button is clicked after dismissal. Manages object deletion in the AR demo scene,
/// and also handles the toggling between the object creation menu button and the delete button.
/// </summary>
public class ARTemplateMenuManager : MonoBehaviour
{
    [SerializeField] [Tooltip("The plane prefab with shadows and debug visuals.")]
    GameObject m_DebugPlane;

    /// <summary>
    /// The plane prefab with shadows and debug visuals.
    /// </summary>
    public GameObject debugPlane
    {
        get => m_DebugPlane;
        set => m_DebugPlane = value;
    }

    [SerializeField] [Tooltip("The plane manager in the AR demo scene.")]
    ARPlaneManager m_PlaneManager;

    /// <summary>
    /// The plane manager in the AR demo scene.
    /// </summary>
    public ARPlaneManager planeManager
    {
        get => m_PlaneManager;
        set => m_PlaneManager = value;
    }

    private bool m_DebugPlaneSlider = true;

    readonly List<ARFeatheredPlaneMeshVisualizerCompanion> featheredPlaneMeshVisualizerCompanions =
        new List<ARFeatheredPlaneMeshVisualizerCompanion>();

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    void OnEnable()
    {
        m_PlaneManager.planesChanged += OnPlaneChanged;
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    void OnDisable()
    {
        m_PlaneManager.planesChanged -= OnPlaneChanged;
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    void Start()
    {
        // Auto turn on/off debug menu. We want it initially active so it calls into 'Start', which will
        // allow us to move the menu properties later if the debug menu is turned on.
        m_PlaneManager.planePrefab = m_DebugPlane;
    }


    /// <summary>
    /// Shows or hides the plane debug visuals.
    /// </summary>
    public void ShowDebugPlane(bool show)
    {
        m_DebugPlaneSlider = show;
        ChangePlaneVisibility(show);

        // Also update the visibility of any existing planes
        foreach (var plane in m_PlaneManager.trackables)
        {
            if (plane.gameObject.activeInHierarchy)
            {
                plane.gameObject.SetActive(show);
            }
        }
    }

    void ChangePlaneVisibility(bool setVisible)
    {
        var count = featheredPlaneMeshVisualizerCompanions.Count;
        for (int i = 0; i < count; ++i)
        {
            var companion = featheredPlaneMeshVisualizerCompanions[i];
            if (companion != null)
            {
                companion.visualizeSurfaces = setVisible;
                companion.gameObject.SetActive(setVisible);
            }
        }
    }


    void OnPlaneChanged(ARPlanesChangedEventArgs eventArgs)
    {
        if (eventArgs.added.Count > 0)
        {
            foreach (var plane in eventArgs.added)
            {
                if (plane.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                {
                    featheredPlaneMeshVisualizerCompanions.Add(visualizer);
                    visualizer.visualizeSurfaces = m_DebugPlaneSlider;
                }
            }
        }

        if (eventArgs.removed.Count > 0)
        {
            foreach (var plane in eventArgs.removed)
            {
                if (plane.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                    featheredPlaneMeshVisualizerCompanions.Remove(visualizer);
            }
        }

        // Fallback if the counts do not match after an update
        if (m_PlaneManager.trackables.count != featheredPlaneMeshVisualizerCompanions.Count)
        {
            featheredPlaneMeshVisualizerCompanions.Clear();
            foreach (var trackable in m_PlaneManager.trackables)
            {
                if (trackable.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                {
                    featheredPlaneMeshVisualizerCompanions.Add(visualizer);
                    visualizer.visualizeSurfaces = m_DebugPlaneSlider;
                }
            }
        }
    }
}