using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // Inspector editable fields
    public Transform[] waypoints;
    public float speedOnPath;
    public float speedSinWobble;
    public float sinAttitude;
    public bool ignorePathY;

    // Store coroutine instances to be able to stop them safely
    private Coroutine _runningMoveOnPath;
    private Coroutine _runningMoveSinWobble;

    private void Start()
    {
        // Start both coroutines
        _runningMoveOnPath = StartCoroutine(MoveOnPath(speedOnPath));
        _runningMoveSinWobble = StartCoroutine(MoveSinWobble(speedSinWobble, sinAttitude));
    }

    private void StopMovement()
    {
        // Stop both coroutines using the stored instances
        StopCoroutine(_runningMoveOnPath);
        StopCoroutine(_runningMoveSinWobble);
    }
    
    private IEnumerator MoveToXYZ(Vector3 destination, float speed)
    {
        // As long as not at the destination position yet
        while (transform.position != destination)
        {
            // Move self towards it in 3d space (XYZ)
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            
            // Return control to engine for now, continue here during next frame
            yield return null;
        }
    }
    
    private IEnumerator MoveToXZ(Vector3 destination, float speed)
    {
        // Convert to 2d space (XZ)
        var destination2d = new Vector2(destination.x, destination.z);
        var position2d = new Vector2(transform.position.x, transform.position.z);
        
        // As long as not at the destination position yet
        while (position2d != destination2d)
        {
            // Move self towards it in 2d space (XZ), Y stays untouched
            position2d = Vector2.MoveTowards(position2d, destination2d, Time.deltaTime * speed);
            transform.position = new Vector3(position2d.x, transform.position.y, position2d.y);
            
            // Return control to engine for now, continue here during next frame
            yield return null;
        }
    }

    private IEnumerator MoveOnPath(float speed)
    {
        // Because we are in a coroutine, this is fine
        while (true)
        {
            // Go through every waypoint one after the other
            foreach (var waypoint in waypoints)
            {
                // Check which movement to use
                if (ignorePathY)
                {
                    // Move in 2d space only (XZ)
                    yield return StartCoroutine(MoveToXZ(waypoint.transform.position, speed));
                }
                else
                {
                    // Move in 3d space (XYZ)
                    yield return StartCoroutine(MoveToXYZ(waypoint.transform.position, speed));
                }
            }
        }
    }
    
    private IEnumerator MoveSinWobble(float speed, float attitude)
    {
        // Store current radians and original Y position
        var radians = 0f;
        var originY = transform.position.y;
        
        // Every frame
        while (true)
        {
            // Update radians and compute new Y offset
            var pos = transform.position;
            radians += Time.deltaTime * speed;
            var offset = Mathf.Sin(radians) * attitude;
            
            // Apply Y offset to position
            transform.position = new Vector3(pos.x, originY + offset, pos.z);
            
            // Return control to engine for now, continue here during next frame
            yield return null;
        }
    }
}
