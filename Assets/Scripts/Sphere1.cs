using UnityEngine;

// Update()-based alpha change implementation (pls no)
public class Sphere1 : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public float alphaDuration;
    public float waitDuration;
    
    // Current state of the animation, and the time passed in that state
    private AlphaAniState _state = AlphaAniState.None;
    private float _timePassed = 0f;
    
    // Enum for the different possible animation states
    private enum AlphaAniState
    {
        None, // Animation not running
        Decr, // Currently decreasing the alpha
        Wait, // Currently waiting in between
        Incr  // Currently increasing the alpha
    }

    private void OnMouseDown()
    {
        // Start the animation by setting the state and resetting the time
        _state = AlphaAniState.Decr;
        _timePassed = 0f;
    }

    private void Update()
    {
        // Do action based on current animation state
        switch (_state)
        {
            // Alpha is decreasing
            case AlphaAniState.Decr:
                
                // Add the time since last frame
                _timePassed += Time.deltaTime;
                
                // Calculate new alpha value and apply it to the renderer
                meshRenderer.material.color = new Color(1f, 1f, 1f, 1f - _timePassed / alphaDuration);
                
                // If finished decreasing, switch to next state and reset time
                if (_timePassed > alphaDuration)
                {
                    _state = AlphaAniState.Wait;
                    _timePassed = 0f;
                }
                
                // We are done for this frame
                break;
            
            // Waiting in between
            case AlphaAniState.Wait:
                
                // Add the time since last frame
                _timePassed += Time.deltaTime;
                
                // If finished waiting, switch to next state and reset time
                if (_timePassed > waitDuration)
                {
                    _state = AlphaAniState.Incr;
                    _timePassed = 0f;
                }
                
                // We are done for this frame
                break;
            
            // Alpha is increasing
            case AlphaAniState.Incr:
                
                // Add the time since last frame
                _timePassed += Time.deltaTime;
                
                // Calculate new alpha value and apply it to the renderer
                meshRenderer.material.color = new Color(1f, 1f, 1f, _timePassed / alphaDuration);
                
                // If finished increasing, reset state and time
                if (_timePassed > alphaDuration)
                {
                    _state = AlphaAniState.None;
                    _timePassed = 0f;
                }
                
                // We are done for this frame
                break;
        }
    }
}
