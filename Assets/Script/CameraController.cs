using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [Header("Follow Target")]
  public Transform target;
  [Header("MovementClamp")]
  public  Vector2 minBounds;
  public  Vector2 maxBounds;
  
  public float cameraMoveSpeed;
  
  public Vector3 offset; // 카메라 오프셋

  void LateUpdate()
  {
    Vector3 desiredPosition = target.position + offset;
    
    // 카메라 위치를 제한
    float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
    float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
    Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

    // 부드럽게 이동
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, cameraMoveSpeed);
    transform.position = smoothedPosition;
    
  }
  
}
