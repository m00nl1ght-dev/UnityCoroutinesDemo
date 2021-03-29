using System.Collections;
using UnityEngine;

// Coroutine-based alpha change implementation with AnimationCurve
public class Sphere3 : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public AnimationCurve animationCurve;
    public float duration;

    // Store the currently running coroutine so we can stop it
    private Coroutine _aniRunning;
    
    private void OnMouseDown()
    {
        // When sphere is clicked, first stop already running coroutine
        if (_aniRunning != null) StopCoroutine(_aniRunning);
        
        // Then start the new one
        _aniRunning = StartCoroutine(AlphaAni());
    }

    private IEnumerator AlphaAni()
    {
        // How much time has the animation been running for
        var timePassed = 0f;
        
        // While not finished
        while (timePassed < duration)
        {
            // Add the time since last frame
            timePassed += Time.deltaTime;
            
            // Calculate new alpha value using the AnimationCurve
            var alpha = animationCurve.Evaluate(timePassed / duration);
            
            // Apply the new value to the renderer
            meshRenderer.material.color = new Color(1f, 1f, 1f, alpha);
            
            // Return control to engine for now, continue here during next frame
            yield return null;
        }
    }
}
