using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private LayerMask layerMask;

    private void Update()
    {
        //shoots out a ray from the top camera that bounces off of anything with the ground layer
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point + offSet;
        }
    }
}
