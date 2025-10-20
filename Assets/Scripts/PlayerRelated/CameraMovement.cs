using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Bounds cameraBounds;
    private Vector3 targetPosition;
    private Camera _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();
        var height = _camera.orthographicSize;
        var width = height*_camera.aspect;

        var minX = Globals.WorldBounds.min.x + width/2;
        var maxX = Globals.WorldBounds.max.x - width/2;

        var minY = Globals.WorldBounds.min.y + height/2;
        var maxY = Globals.WorldBounds.max.y - height/2;

        cameraBounds = new Bounds();
        cameraBounds.SetMinMax(new Vector3(minX, minY, -60f), new Vector3(maxX,maxY,-60f));
    }
    void Update()
    {
        targetPosition = transform.parent.position;
        
        transform.position = new Vector3(Mathf.Clamp(targetPosition.x, cameraBounds.min.x, cameraBounds.max.x), Mathf.Clamp(targetPosition.y, cameraBounds.min.y, cameraBounds.max.y), -60f);
    }

}
