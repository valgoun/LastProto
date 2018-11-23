using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoomTypeEnum
{
    LIMIT,
    HOVER_LIMIT,
    HOVER
}

public class CameraController : MonoBehaviour {

    [Header("Plane Movement")]
    public float LeftZone;
    public float RightZone;
    public float TopZone;
    public float BottomZone;

    [Space]
    public bool ZoneUseGradient;
    public float ZoneMinForce;
    public float ZoneMaxForce;

    [Space]
    public float MaximumSpeed;
    public float Acceleration;
    public float Drag;
    public float VelocityThreshold;
    public float MaxZoomMultiplier;

    [Space]
    public LayerMask BoundariesLayer;
    public float DeccelerationGradient;
    public float DistanceThresholdForDecceleration;
    public float SafeDistanceFromBound;

    [Header("Zoom")]
    public float ZoomInputForce;

    [Space]
    public float ZoomMovementForce;
    public float ZoomMovementThreshold;

    [Space]
    public LayerMask FloorLayer;

    [Space]
    public ZoomTypeEnum MinZoomType;
    public float MinZoomLimit;
    public float MinZoomHover;

    [Space]
    public ZoomTypeEnum MaxZoomType;
    public float MaxZoomLimit;
    public float MaxZoomHover;

    [Space]
    public float ZoomAngleForce;
    public float ZoomAngleThreshold;

    [Space]
    public Vector3 MinAngle;
    public Vector3 MaxAngle;


    //PRIVATE
    private Vector3 _planeVelocity;
    private float _zoomLevel = 0;
    private Vector3 _zoomVelocity;
    private Vector3 _angularVelocity;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Zoom Input
        _zoomLevel = Mathf.Clamp(_zoomLevel + Input.GetAxis("Mouse ScrollWheel") * ZoomInputForce * Time.deltaTime, 0, 1);

        //Zoom Movement
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, FloorLayer);

        float min = GetLimit(hit, MinZoomType, MinZoomLimit, MinZoomHover);
        float max = GetLimit(hit, MaxZoomType, MaxZoomLimit, MaxZoomHover);

        float idealHeight = Mathf.Lerp(min, max, _zoomLevel);

        if ((transform.position.y >= idealHeight - ZoomMovementThreshold) && (transform.position.y <= idealHeight + ZoomMovementThreshold))
        {
            _zoomVelocity = Vector3.zero;
        }
        else
        {
            _zoomVelocity = new Vector3(0, 1, 0) * (idealHeight - transform.position.y) * ZoomMovementForce;
        }

        //Zoom Rotation
        Vector3 idealRotation = new Vector3(Mathf.Lerp(MinAngle.x, MaxAngle.x, _zoomLevel), Mathf.Lerp(MinAngle.y, MaxAngle.y, _zoomLevel), Mathf.Lerp(MinAngle.z, MaxAngle.z, _zoomLevel));
        Vector3 rot = transform.eulerAngles;

        if (((rot.x >= idealRotation.x - ZoomAngleThreshold) && (rot.x <= idealRotation.x + ZoomAngleThreshold))
            && ((rot.y >= idealRotation.y - ZoomAngleThreshold) && (rot.y <= idealRotation.y + ZoomAngleThreshold))
            && ((rot.z >= idealRotation.z - ZoomAngleThreshold) && (rot.z <= idealRotation.z + ZoomAngleThreshold)))
        {
            _angularVelocity = Vector3.zero;
        }
        else
        {
            _angularVelocity = (idealRotation - rot) * ZoomAngleForce;
        }

        //Plane Movement Input
        Vector3 input = Vector3.zero;

        float gradient = 1 - (Input.mousePosition.y / (BottomZone * Screen.height));
        if (gradient >= 0)
        {
            input += new Vector3(0, 0, -gradient);
        }
        else
        {
            gradient = (Input.mousePosition.y - (1 - TopZone) * Screen.height) / (TopZone * Screen.height);
            if (gradient >= 0)
            {
                input += new Vector3(0, 0, gradient);
            }
        }

        gradient = 1 - (Input.mousePosition.x / (LeftZone * Screen.width));
        if (gradient >= 0)
        {
            input += new Vector3(-gradient, 0, 0);
        }
        else
        {
            gradient = (Input.mousePosition.x - (1 - RightZone) * Screen.width) / (RightZone * Screen.width);
            if (gradient >= 0)
            {
                input += new Vector3(gradient, 0, 0);
            }
        }

        //Plane Movement
        float zoomGradient = Mathf.Lerp(1, MaxZoomMultiplier, _zoomLevel);
        _planeVelocity /= (1 + Drag * Time.deltaTime);

        float maxSpeed = MaximumSpeed * zoomGradient;
        if (Physics.Raycast(transform.position, ((_planeVelocity.magnitude > 0f) ? _planeVelocity : input).normalized, out hit, DistanceThresholdForDecceleration + SafeDistanceFromBound, BoundariesLayer))
        {
            float dist = hit.distance - SafeDistanceFromBound;
            if (dist <= 0)
            {
                maxSpeed = 0;
            }
            else
            {
                float size = input.magnitude;
                if (size < 1)
                    maxSpeed *= size;

                if (dist <= DistanceThresholdForDecceleration)
                {
                    float sq = (dist / DistanceThresholdForDecceleration);
                    maxSpeed *= Mathf.Pow(sq, DeccelerationGradient);
                }
            }

            _planeVelocity += Acceleration * input;
            _planeVelocity = (_planeVelocity.magnitude > maxSpeed) ? (_planeVelocity).normalized * maxSpeed : _planeVelocity;
        }
        else
        {
            float size = input.magnitude;
            if (size < 1)
                maxSpeed *= size;

            if (_planeVelocity.magnitude < maxSpeed)
            {
                _planeVelocity += Acceleration * input;
                if (_planeVelocity.magnitude > maxSpeed)
                {
                    _planeVelocity = _planeVelocity.normalized * maxSpeed;
                }
            }
        }

        _planeVelocity = (_planeVelocity.magnitude <= VelocityThreshold * zoomGradient) ? Vector3.zero : _planeVelocity;

        //Apply velocities
        transform.position += (_planeVelocity + _zoomVelocity) * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(_angularVelocity * Time.deltaTime);
    }

    private float GetLimit(RaycastHit hit, ZoomTypeEnum type, float limit, float hover)
    {
        float value;
        if (type == ZoomTypeEnum.HOVER_LIMIT || type == ZoomTypeEnum.HOVER)
        {
            value = hit.point.y + hover;
            if (type == ZoomTypeEnum.HOVER_LIMIT && (value < limit))
            {
                value = limit;
            }
        }
        else
            value = limit;

        return value;
    }
}
