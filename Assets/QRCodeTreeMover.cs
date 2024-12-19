using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class QRCodeTreeMover : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager; // Reference to the ARTrackedImageManager component
    public GameObject tree; // Reference to your tree object that will move

    // Start is called before the first frame update
    void Start()
    {
        // Check if ARTrackedImageManager is assigned and register for image tracking updates
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    // This method is called whenever tracked images change (i.e., QR code is detected or lost)
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Loop through the added images (this will trigger when a new QR code is detected)
        foreach (var addedImage in args.added)
        {
            // Check if the image is being tracked
            if (addedImage.trackingState == TrackingState.Tracking)
            {
                // Move the tree to the position of the detected QR code
                MoveTreeToQRCode(addedImage);
            }
        }
    }

    // This method sets the tree's position to the QR code's detected position
    private void MoveTreeToQRCode(ARTrackedImage trackedImage)
    {
        // Move the tree to the detected QR code's position in world space
        tree.transform.position = trackedImage.transform.position;
    }
}