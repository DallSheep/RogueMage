using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlayer : MonoBehaviour
{
    [SerializeField] private Transform _displayAnchor;
    [SerializeField] private Camera _cam;
    [SerializeField] private RectTransform _display;
    
    private Transform _pivot;

    [SerializeField] private float _rotSens = 15;
    [SerializeField] private float _minRotX = -360;
    [SerializeField] private float _maxRotX = 360;
    [SerializeField] private float _minRotY = -20;
    [SerializeField] private float _maxRotY = 20;

    private float _rotX;
    private float _rotY;
    private Quaternion _ogRot;

    private Vector2 _textureDim;

    void Start()
    {
        _pivot = _displayAnchor.GetChild(0);
        _textureDim = new Vector3(_display.GetComponent<RawImage>().texture.width,
                                  _display.GetComponent<RawImage>().texture.height);

        _ogRot = _pivot.localRotation;
    }

    void Update()
    {
        Vector3 btmLftCorner = new Vector3(_display.position.x - (_display.sizeDelta.x / 2),
                                           _display.position.y - (_display.sizeDelta.y / 2));

        CastRay(_cam, btmLftCorner, _display.sizeDelta, _textureDim);
    }

    private RaycastHit CastRay(Camera cam, Vector3 btmLftCorner, Vector3 sizeDelta, Vector2 textureDim)
    {
        Vector3 relMousePos = Input.mousePosition - btmLftCorner;
        relMousePos.x = ((relMousePos.x / sizeDelta.x) * _textureDim.x);
        relMousePos.y = ((relMousePos.y / sizeDelta.y) * _textureDim.y);

        RaycastHit hit;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(cam.transform.position, hit.point, Color.blue);
        }

        return hit;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle > 360)
            angle -= 360;

        if (angle < -360)
            angle += 360;

        return Mathf.Clamp(angle, min, max);
    }
}
