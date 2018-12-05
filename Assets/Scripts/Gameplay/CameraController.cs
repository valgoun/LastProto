using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ZoomTypeEnum
{
    LIMIT,
    HOVER_LIMIT,
    HOVER
}

public class CameraController : MonoBehaviour {

    [TabGroup("Plane Input")]
    [Tooltip("Size of the zone in Screen ratio, originating from the Left side and going to the Right side.")]
    public float LeftZone;
    [TabGroup("Plane Input")]
    [Tooltip("Size of the zone in Screen ratio, originating from the Right side and going to the Left side.")]
    public float RightZone;
    [TabGroup("Plane Input")]
    [Tooltip("Size of the zone in Screen ratio, originating from the Top and going to the Bottom.")]
    public float TopZone;
    [TabGroup("Plane Input")]
    [Tooltip("Size of the zone in Screen ratio, originating from the Bottom and going to the Top.")]
    public float BottomZone;
    [TabGroup("Plane Input"), Space]
    [Tooltip("If true, the input strength will depend of the distance of the cursor from the origin when it's in a zone. Otherwise, the input will always be flat 1.")]
    public bool ZoneUseGradient;

    [TabGroup("Plane Movement")]
    public float MaximumSpeed;
    [TabGroup("Plane Movement")]
    public float Acceleration;
    [TabGroup("Plane Movement")]
    public float Drag;
    [TabGroup("Plane Movement")]
    public float VelocityThreshold;

    [TabGroup("Plane Movement"), Space]
    [Tooltip("Speed multiplier when the Camera is at Max Zoom. Useful for slowing down the speed of the Camera when the Camera is zoomed in to make it more enjoyable to control.")]
    public float MaxZoomMultiplier;
    [TabGroup("Plane Movement")]
    [Tooltip("Speed multiplier when the camera is centering on selection. Useful to make camera feel more responsive when using the shortcuts.")]
    public float CameraCenteringMultiplier = 3.5f;
    [TabGroup("Plane Movement")]
    [Tooltip("Distance at which the Camera starts decelerating when centering on selection. Allow to smooth more or less the centering movement (bigger value = more smoothing).")]
    public float CameraCenteringDeccelerationTreshold = 5;

    [TabGroup("Plane Movement"), Space]
    public LayerMask BoundariesLayer;
    [TabGroup("Plane Movement")]
    public float DeccelerationGradient;
    [TabGroup("Plane Movement")]
    public float DistanceThresholdForDecceleration;
    [TabGroup("Plane Movement")]
    public float SafeDistanceFromBound;

    [TabGroup("Zoom")]
    public float ZoomInputForce;

    [TabGroup("Zoom"), Space]
    public float ZoomMovementForce;
    [TabGroup("Zoom")]
    public float ZoomMovementThreshold;

    [TabGroup("Zoom"), Space]
    public LayerMask FloorLayer;

    [TabGroup("Zoom"), Space]
    public ZoomTypeEnum MinZoomType;
    [TabGroup("Zoom"), ShowIf("IsMinLimit")]
    public float MinZoomLimit;
    [TabGroup("Zoom"), ShowIf("IsMinHover")]
    public float MinZoomHover;

    [TabGroup("Zoom"), Space]
    public ZoomTypeEnum MaxZoomType;
    [TabGroup("Zoom"), ShowIf("IsMaxLimit")]
    public float MaxZoomLimit;
    [TabGroup("Zoom"), ShowIf("IsMaxHover")]
    public float MaxZoomHover;

    [TabGroup("Zoom"), Space]
    public float ZoomAngleForce;
    [TabGroup("Zoom")]
    public float ZoomAngleThreshold;
    [TabGroup("Zoom")]
    public Vector3 MinAngle;
    [TabGroup("Zoom")]
    public Vector3 MaxAngle;

    [Header("Debug")]
    [ReadOnly]
    public Vector3 PlaneVelocity;
    [Space, ReadOnly]
    public float ZoomLevel = 0;
    [ReadOnly]
    public Vector3 ZoomVelocity;
    [ReadOnly]
    public Vector3 AngularVelocity;

    private bool IsMinLimit { get { return (MinZoomType == ZoomTypeEnum.LIMIT || MinZoomType == ZoomTypeEnum.HOVER_LIMIT); } }
    private bool IsMaxLimit { get { return (MaxZoomType == ZoomTypeEnum.LIMIT || MaxZoomType == ZoomTypeEnum.HOVER_LIMIT); } }
    private bool IsMinHover { get { return (MinZoomType == ZoomTypeEnum.HOVER || MinZoomType == ZoomTypeEnum.HOVER_LIMIT); } }
    private bool IsMaxHover { get { return (MaxZoomType == ZoomTypeEnum.HOVER || MaxZoomType == ZoomTypeEnum.HOVER_LIMIT); } }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Zoom Input
        ZoomLevel = Mathf.Clamp(ZoomLevel + Input.GetAxis("Mouse ScrollWheel") * ZoomInputForce * Time.deltaTime, 0, 1);

        //Zoom Movement
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, FloorLayer);

        float min = GetLimit(hit, MinZoomType, MinZoomLimit, MinZoomHover);
        float max = GetLimit(hit, MaxZoomType, MaxZoomLimit, MaxZoomHover);

        float idealHeight = Mathf.Lerp(min, max, ZoomLevel);

        if ((transform.position.y >= idealHeight - ZoomMovementThreshold) && (transform.position.y <= idealHeight + ZoomMovementThreshold))
        {
            ZoomVelocity = Vector3.zero;
        }
        else
        {
            ZoomVelocity = new Vector3(0, 1, 0) * (idealHeight - transform.position.y) * ZoomMovementForce;
        }

        //Zoom Rotation
        Vector3 idealRotation = new Vector3(Mathf.Lerp(MinAngle.x, MaxAngle.x, ZoomLevel), Mathf.Lerp(MinAngle.y, MaxAngle.y, ZoomLevel), Mathf.Lerp(MinAngle.z, MaxAngle.z, ZoomLevel));
        Vector3 rot = transform.eulerAngles;

        if (((rot.x >= idealRotation.x - ZoomAngleThreshold) && (rot.x <= idealRotation.x + ZoomAngleThreshold))
            && ((rot.y >= idealRotation.y - ZoomAngleThreshold) && (rot.y <= idealRotation.y + ZoomAngleThreshold))
            && ((rot.z >= idealRotation.z - ZoomAngleThreshold) && (rot.z <= idealRotation.z + ZoomAngleThreshold)))
        {
            AngularVelocity = Vector3.zero;
        }
        else
        {
            AngularVelocity = (idealRotation - rot) * ZoomAngleForce;
        }

        //Plane Movement Input
        Vector3 input = Vector3.zero;
        float centeringGradient = 1;

        int centeringLength = 0;
        Vector3 point = Vector3.zero;
        foreach (Unit unit in SelectionManager.Instance.SelectedElements)
        {
            if (unit)
            {
                point += unit.transform.position;
                centeringLength++;
            }
        }

        if (!Input.GetButton("Centering") || (centeringLength == 0))
        {
            Vector3 verticalInput = Vector3.zero;
            float gradient = 1 - (Input.mousePosition.y / (BottomZone * Screen.height));
            if (gradient >= 0)
            {
                verticalInput = new Vector3(0, 0, -((ZoneUseGradient) ? gradient : 1));
            }
            else
            {
                gradient = (Input.mousePosition.y - (1 - TopZone) * Screen.height) / (TopZone * Screen.height);
                if (gradient >= 0)
                {
                    verticalInput = new Vector3(0, 0, ((ZoneUseGradient) ? gradient : 1));
                }
            }
            if (verticalInput == Vector3.zero)
            {
                input += new Vector3(0, 0, Input.GetAxis("Vertical"));
            }
            else
                input += verticalInput;

            Vector3 horizontalInput = Vector3.zero;
            gradient = 1 - (Input.mousePosition.x / (LeftZone * Screen.width));
            if (gradient >= 0)
            {
                input += new Vector3(-((ZoneUseGradient) ? gradient : 1), 0, 0);
            }
            else
            {
                gradient = (Input.mousePosition.x - (1 - RightZone) * Screen.width) / (RightZone * Screen.width);
                if (gradient >= 0)
                {
                    horizontalInput = new Vector3(((ZoneUseGradient) ? gradient : 1), 0, 0);
                }
            }
            if (horizontalInput == Vector3.zero)
            {
                input += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            }
            else
                input += horizontalInput;
        }
        else
        {
            centeringGradient = CameraCenteringMultiplier;

            point /= centeringLength;

            Vector3 dir = point - transform.position;
            dir.y = 0;
            input = dir.normalized;
            if (dir.magnitude < CameraCenteringDeccelerationTreshold)
            {
                centeringGradient *= (dir.magnitude / CameraCenteringDeccelerationTreshold);
            }
        }

        //Plane Movement
        float speedGradient = Mathf.Lerp(1, MaxZoomMultiplier, ZoomLevel) * centeringGradient;
        PlaneVelocity /= (1 + (Drag/centeringGradient) * Time.deltaTime);

        float maxSpeed = MaximumSpeed * speedGradient;
        if (Physics.Raycast(transform.position, ((PlaneVelocity.magnitude > 0f) ? PlaneVelocity : input).normalized, out hit, DistanceThresholdForDecceleration + SafeDistanceFromBound, BoundariesLayer))
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

            PlaneVelocity += Acceleration * input;
            PlaneVelocity = (PlaneVelocity.magnitude > maxSpeed) ? (PlaneVelocity).normalized * maxSpeed : PlaneVelocity;
        }
        else
        {
            float size = input.magnitude;
            if (size < 1)
                maxSpeed *= size;

            if (PlaneVelocity.magnitude < maxSpeed)
            {
                PlaneVelocity += Acceleration * input;
                if (PlaneVelocity.magnitude > maxSpeed)
                {
                    PlaneVelocity = PlaneVelocity.normalized * maxSpeed;
                }
            }
        }

        PlaneVelocity = (PlaneVelocity.magnitude <= VelocityThreshold * speedGradient) ? Vector3.zero : PlaneVelocity;

        //Apply velocities
        transform.position += (PlaneVelocity + ZoomVelocity) * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(AngularVelocity * Time.deltaTime);
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
