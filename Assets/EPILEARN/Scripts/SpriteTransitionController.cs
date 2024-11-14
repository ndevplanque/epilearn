using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpriteTransitionController : MonoBehaviour
{
    [Header("Sprite Configuration")] public List<Sprite> spriteSequence;

    public Image displayImage;
    public float transitionDuration = 0.5f;

    [Header("Overlay for color transitions")] [Tooltip("An overlay used to transition colors.")] [SerializeField]
    private Image colorOverlay;

    [Header("End Action")] public UnityEvent onSequenceComplete;

    [Tooltip("The greeting prompt Game Object to show when onboarding begins.")] [SerializeField]
    private GameObject m_GreetingPrompt;

    [Tooltip("Intro prompt.")] [SerializeField]
    private GameObject IntroPrompt;

    // Color fade effect
    [Tooltip("Color fade effect")] [SerializeField]
    private Color fadeColor = Color.black;

    private int currentSpriteIndex;
    private bool isInitialized;

    public void Initialize()
    {
        m_GreetingPrompt.SetActive(false);

        if (spriteSequence == null || spriteSequence.Count == 0)
        {
            Debug.LogError("The sprite list is empty. Please configure the sprites.");
            return;
        }

        if (displayImage == null)
        {
            Debug.LogError("No reference image has been assigned.");
            return;
        }

        if (colorOverlay == null)
        {
            Debug.LogError("No reference overlay has been assigned.");
            return;
        }

        // Ensure the color overlay starts as opaque
        colorOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);

        // Show first sprite then fade in
        displayImage.sprite = spriteSequence[0];
        StartCoroutine(TransitionFromOverlayToSprite());

        isInitialized = true;
    }

    public void ShowNextSprite()
    {
        if (!isInitialized)
        {
            Debug.LogError("The controller is not initialized. Call Initialize() first.");
            return;
        }

        currentSpriteIndex++;

        if (currentSpriteIndex >= spriteSequence.Count)
        {
            StartCoroutine(TransitionToTransparentAndEnd());
            return;
        }

        StartCoroutine(TransitionToNextSprite());
    }

    private IEnumerator TransitionFromOverlayToSprite()
    {
        // Ensure the start of the overlay fade transition
        yield return StartCoroutine(FadeOverlayToAlpha(0));

        // Ensure the final state is completely opaque
        displayImage.color = new Color(1, 1, 1, 1);
    }

    private IEnumerator TransitionToNextSprite()
    {
        yield return StartCoroutine(FadeOverlayToAlpha(1)); // Fade overlay to specified color

        displayImage.sprite = spriteSequence[currentSpriteIndex];

        yield return StartCoroutine(FadeOverlayToAlpha(0)); // Fade overlay back to transparency
    }

    private IEnumerator TransitionToTransparentAndEnd()
    {
        // Ensure both overlay and image start fully opaque
        colorOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);
        displayImage.color = new Color(1, 1, 1, 1);

        // Fade both the overlay and the image out
        yield return StartCoroutine(FadeOverlayAndImageToTransparent());

        // Invoke the completion event
        onSequenceComplete?.Invoke();
        IntroPrompt.SetActive(false);
    }


    private IEnumerator FadeOverlayAndImageToTransparent()
    {
        var elapsedTime = 0f;
        var initialOverlayAlpha = colorOverlay.color.a;
        var initialImageAlpha = displayImage.color.a;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            var newOverlayAlpha = Mathf.Lerp(initialOverlayAlpha, 0f, elapsedTime / transitionDuration);
            var newImageAlpha = Mathf.Lerp(initialImageAlpha, 0f, elapsedTime / transitionDuration);

            colorOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, newOverlayAlpha);
            displayImage.color = new Color(1, 1, 1, newImageAlpha);

            yield return null;
        }

        // Ensure both are completely transparent at the end
        colorOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        displayImage.color = new Color(1, 1, 1, 0f);
    }


    private IEnumerator FadeOverlayToAlpha(float targetAlpha)
    {
        var elapsedTime = 0f;
        var initialAlpha = colorOverlay.color.a;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            var newAlpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / transitionDuration);
            colorOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, newAlpha);
            yield return null;
        }

        // Ensure target alpha is set
        colorOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, targetAlpha);
    }
}