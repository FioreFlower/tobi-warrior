using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  
  private Transform _target;
  
  [Header("MovementClamp")]
  public  Vector2 minBounds;
  public  Vector2 maxBounds;

  private bool _canMove = false;
  public float cameraMoveSpeed;
  
  public Vector3 offset; // 카메라 오프셋

  void Start()
  {
    
    StartCoroutine(Countdown());
  }

  IEnumerator Countdown()
  {
    yield return new WaitForSeconds(3);
    _canMove = true;
    _target = GameObject.FindGameObjectWithTag("Player").transform;
  }

  void LateUpdate()
  {
    if (_canMove)
    {
      if(!_target) return;
      CameraMovement();
    }
  }

  public void SetTarget(Transform newTarget)
  {
    _target = newTarget;
  }

  void CameraMovement()
  {
    Vector3 desiredPosition = _target.position + offset;
    
    // 카메라 위치를 제한
    float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
    float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
    Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

    // 부드럽게 이동
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, cameraMoveSpeed);
    transform.position = smoothedPosition;
  }

}
