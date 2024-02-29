using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AdvancedPeopleSystem;

// Transition manager based on Smooth Scene Fade Transition in VR
// Valem Tutorials
// https://www.youtube.com/watch?v=JCyJ26cIM0Y
// Accessed on 29/02/2024

// Manages the transition between scenes
public class TransitionManager : MonoBehaviour
{
    public GameObject fadeScreen;
    public bool fadeOnstart = true;
    public float fadeDuration;
    public Color fadeColor;
    private Renderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = fadeScreen.GetComponent<Renderer>();
        if (fadeOnstart) FadeIn();

        // Initialise character ssettings on scene transition
        CharacterSystemUpdater.UpdateCharactersOnScene();
    }

    // Calls a fade in transition
    public void FadeIn()
    {
        Fade(1, 0);
    }

    // Calls a fade out transition
    public void FadeOut()
    {
        Fade(0, 1);
    }

    // Takes two alpha values and starts a transition between them
    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    // Coroutine to manage the transition
    IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;

        while(timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            meshRenderer.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        meshRenderer.material.SetColor("_BaseColor", newColor2);
    }

    // Go to a different scene with fade transition
    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(SceneRoutine(sceneIndex));
    }

    IEnumerator SceneRoutine(int sceneIndex)
    {
        FadeOut();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        float timer = 0;
        while (timer <= fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
