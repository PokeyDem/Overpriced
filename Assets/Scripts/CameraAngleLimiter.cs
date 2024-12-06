using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraAngleLimiter : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector2 pitchLimits = new Vector2(-30f, 30f);
    public Vector2 yawLimits = new Vector2(-60f, 60f);

    private float pitch;
    private float yaw;

    void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned. Please assign the camera's Transform.");
        }
        else
        {
           
            Vector3 initialRotation = cameraTransform.eulerAngles;
            pitch = initialRotation.x;
            yaw = initialRotation.y;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
           
            Vector3 currentRotation = cameraTransform.eulerAngles;

            
            pitch = ClampAngle(currentRotation.x, pitchLimits.x, pitchLimits.y);
            yaw = ClampAngle(currentRotation.y, yawLimits.x, yawLimits.y);

         
            cameraTransform.rotation = Quaternion.Euler(pitch, yaw, currentRotation.z);
        }
    }
    
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle > 180) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
