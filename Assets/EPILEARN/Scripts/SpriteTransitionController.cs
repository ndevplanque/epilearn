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

    [Header("End Action")] public UnityEvent onSequenceComplete;

    [Tooltip("The greeting prompt Game Object to show when onboarding begins.")] [SerializeField]
    private GameObject m_GreetingPrompt;

    [Tooltip("Intro prompt.")] [SerializeField]
    private GameObject IntroPrompt;

    private int currentSpriteIndex; // Start at 0 for the first sprite
    private bool isInitialized;

    // This method must be called before using the controller
    public void Initialize()
    {
        m_GreetingPrompt.SetActive(false);

        // Initial checks
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

        // Immediately show the first sprite with a fade-in effect
        StartCoroutine(TransitionFromTransparentToSprite(spriteSequence[currentSpriteIndex]));

        isInitialized = true;
    }

    public void ShowNextSprite()
    {
        if (!isInitialized)
        {
            Debug.LogError("The controller is not initialized. Call Initialize() first.");
            return;
        }

        // Increment sprite index
        currentSpriteIndex++;

        // Check if we have reached the end of the sequence
        if (currentSpriteIndex >= spriteSequence.Count)
        {
            // Start the transition to transparent, and then execute end actions
            StartCoroutine(TransitionToTransparentAndEnd());
            return;
        }

        // Start the transition to the next sprite
        StartCoroutine(TransitionToNextSprite());
    }

    private IEnumerator TransitionFromTransparentToSprite(Sprite sprite)
    {
        var elapsedTime = 0f;
        displayImage.sprite = sprite;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            var alpha = Mathf.Clamp01(elapsedTime / transitionDuration);
            displayImage.color = new Color(1, 1, 1, alpha); // Transition from transparent to opaque
            yield return null;
        }
    }

    private IEnumerator TransitionToNextSprite()
    {
        // Get the initial color
        var initialColor = displayImage.color;

        // Fade to black
        yield return StartCoroutine(FadeToColor(Color.black));

        // Change the image
        displayImage.sprite = spriteSequence[currentSpriteIndex];

        // Fade from black back to the initial color
        yield return StartCoroutine(FadeFromColor(Color.black, initialColor));
    }

    private IEnumerator TransitionToTransparentAndEnd()
    {
        var elapsedTime = 0f;
        var startColor = displayImage.color;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            var alpha = 1 - Mathf.Clamp01(elapsedTime / transitionDuration);
            displayImage.color =
                new Color(startColor.r, startColor.g, startColor.b, alpha); // Transition towards transparency
            yield return null;
        }

        // Actions to perform when the sequence is complete
        onSequenceComplete?.Invoke();
        IntroPrompt.SetActive(false);
    }

    private IEnumerator FadeToColor(Color targetColor)
    {
        var initialColor = displayImage.color;
        var elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            // Interpolate the color
            displayImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / transitionDuration);
            yield return null;
        }

        // Ensure target color is set
        displayImage.color = targetColor;
    }

    private IEnumerator FadeFromColor(Color fromColor, Color toColor)
    {
        var elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            // Interpolate the color from one to another
            displayImage.color = Color.Lerp(fromColor, toColor, elapsedTime / transitionDuration);
            yield return null;
        }

        // Ensure final color is set
        displayImage.color = toColor;
    }
}