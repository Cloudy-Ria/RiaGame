using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Bounds cameraBounds;
    private Vector3 targetPosition;
    private Camera _camera;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float height;
    public float width;

    void Start()
    {
        _camera = GetComponent<Camera>();
        //height = _camera.orthographicSize;
        height = Mathf.Abs(transform.position.z* Mathf.Sin(_camera.fieldOfView/2 * Mathf.Deg2Rad) / Mathf.Cos(_camera.fieldOfView / 2 * Mathf.Deg2Rad));
        width = height*_camera.aspect;

        minX = Globals.WorldBounds.min.x + width;
        maxX = Globals.WorldBounds.max.x - width;

        minY = Globals.WorldBounds.min.y + height;
        maxY = Globals.WorldBounds.max.y - height;

        cameraBounds = new Bounds();
        cameraBounds.SetMinMax(new Vector3(minX, minY, -60f), new Vector3(maxX,maxY,-60f));
    }
    void Update()
    {
        targetPosition = transform.parent.position;
        
        transform.position = new Vector3(Mathf.Clamp(targetPosition.x, cameraBounds.min.x, cameraBounds.max.x), Mathf.Clamp(targetPosition.y, cameraBounds.min.y, cameraBounds.max.y), -60f);
    }

}
