using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _followTarget;
    [SerializeField] Vector3 _cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = Camera.main.transform.position;
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _followTarget.position + _cameraOffset;
    }
}
