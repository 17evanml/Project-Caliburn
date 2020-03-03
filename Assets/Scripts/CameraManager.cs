using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [Header("Transform Settings")]
    [SerializeField] private Transform player;

    [Header("Camera Position Settings")]
    [SerializeField] private float playerHeight = 1.7f;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float offsetFromWall = 0.1f;
    [SerializeField] private float maxDistance = 20;
    [SerializeField] private float minDistance = .6f;

    [Header("Camera Speed Settings")]
    [SerializeField] private float speedDistance = 5;
    [SerializeField] private float xSpeed = 200.0f;
    [SerializeField] private float ySpeed = 200.0f;
    [SerializeField] private int zoomRate = 40;

    [Header("Camera Limit and Damping Settings")]
    [SerializeField] private int yMinLimit = -40;
    [SerializeField] private int yMaxLimit = 80;
    [SerializeField] private float rotationDampening = 3.0f;
    [SerializeField] private float zoomDampening = 5.0f;
    public LayerMask collisionLayers = -1;
    [HideInInspector] public Vector3 cameraRotation;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;

    void Start () { setUpCameraRotation(); }
 
    void LateUpdate () {
        cameraRotation = new Vector3(gameObject.transform.localRotation.eulerAngles.x, gameObject.transform.localRotation.eulerAngles.y, gameObject.transform.localRotation.eulerAngles.z);

        Vector3 vTargetOffset, position; Quaternion rotation;
        if (!player) { return; }
        mouseInput();
        scrollInput();

        rotation = Quaternion.Euler(yDeg, xDeg, 0);
        vTargetOffset = new Vector3 (0, -playerHeight, 0);
        position = player.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);
        handleCameraCollision(vTargetOffset, position, rotation);
        setCamera(rotation, position);
    }
 
    private static float ClampAngle (float angle, float min, float max) {
        if (angle < -360) { angle += 360; }
        if (angle > 360) { angle -= 360; }
        return Mathf.Clamp (angle, min, max);
    }

    private void setUpCameraRotation() {
        Vector3 angles = transform.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;
 
        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;
    }

    private void mouseInput() {
        if (GUIUtility.hotControl == 0) {
            xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
        } else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) { // NOTE TO ARJUN: USE VERTICAL AND HORIZONTAL AXIS NOW
            float targetRotationAngle = player.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            xDeg = Mathf.LerpAngle (currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
        }
    }

    private void scrollInput() {
        desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance) * speedDistance;
        desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);

        correctedDistance = desiredDistance;

        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
    }

    private void handleCameraCollision(Vector3 vTargetOffset, Vector3 position, Quaternion rotation) {
        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(player.position.x, player.position.y, player.position.z) - vTargetOffset;

        bool isCorrected = false;
        if (Physics.Linecast (trueTargetPosition, position, out collisionHit, collisionLayers.value)) {
            correctedDistance = Vector3.Distance (trueTargetPosition, collisionHit.point) - offsetFromWall;
            isCorrected = true;
        }

        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp (currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;
        currentDistance = Mathf.Clamp (currentDistance, minDistance, maxDistance);
        position = player.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);
    }

    private void setCamera(Quaternion rotation, Vector3 position) {
        transform.rotation = rotation;
        transform.position = position;
    }
}