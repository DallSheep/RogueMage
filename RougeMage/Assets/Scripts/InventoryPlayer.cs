using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlayer : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private RectTransform _display;
    [SerializeField] private Canvas _canvas;

    private Vector2 _textureDim;

    void Start()
    {
        _cam = GameObject.FindWithTag("Player Cam").GetComponent<Camera>();

        _textureDim = new Vector3(_display.GetComponent<RawImage>().texture.width,
                                  _display.GetComponent<RawImage>().texture.height);
    }

    void Update()
    {
        Vector3 scaledSizeDelta = _display.sizeDelta * _canvas.scaleFactor;
        Vector3 btmLftCorner = new Vector3(_display.position.x - (scaledSizeDelta.x / 2),
                                           _display.position.y - (scaledSizeDelta.y / 2));

        CastRay(_cam, btmLftCorner, scaledSizeDelta, _textureDim);
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
}
