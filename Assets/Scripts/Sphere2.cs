using System.Collections;
using UnityEngine;

// Coroutine-based alpha change implementation
public class Sphere2 : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public float alphaDuration;
    public float waitDuration;

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
        // How much time has the decrease animation been running for
        var decrTime = 0f;
        
        // While not finished decreasing
        while (decrTime < alphaDuration)
        {
            // Add the time since last frame
            decrTime += Time.deltaTime;
            
            // Calculate new alpha value and apply it to the renderer
            meshRenderer.material.color = new Color(1f, 1f, 1f, 1f - decrTime / alphaDuration);
            
            // Return control to engine for now, continue here during next frame
            yield return null;
        }

        // Return control to engine for now, and continue here after waitDuration
        yield return new WaitForSeconds(waitDuration);
        
        // How much time has the increase animation been running for
        var incrTime = 0f;
        
        // While not finished increasing
        while (incrTime < alphaDuration)
        {
            // Add the time since last frame
            incrTime += Time.deltaTime;
            
            // Calculate new alpha value and apply it to the renderer
            meshRenderer.material.color = new Color(1f, 1f, 1f, incrTime / alphaDuration);
            
            // Return control to engine for now, continue here during next frame
            yield return null;
        }
    }
}
