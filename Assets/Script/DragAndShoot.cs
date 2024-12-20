using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
  private Vector2 startDragPosition;
  private Vector2 endDragPosition;
  private Rigidbody2D rb;
  private bool isDragging = false;

  [SerializeField] private float launchForceMultiplier = 10f;
  [SerializeField] private LineRenderer lineRenderer;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  void OnMouseDown()
  {
    startDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    isDragging = true;
  }

  void OnMouseDrag()
  {
    if (!isDragging) return;

    Vector2 currentDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // 시각적 피드백 표시
    if (lineRenderer != null)
    {
      lineRenderer.positionCount = 2;
      lineRenderer.SetPosition(0, startDragPosition);
      lineRenderer.SetPosition(1, currentDragPosition);
    }
  }

  void OnMouseUp()
  {
    if (!isDragging) return;

    endDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    isDragging = false;

    Vector2 launchDirection = (startDragPosition - endDragPosition).normalized;
    float dragDistance = Vector2.Distance(startDragPosition, endDragPosition);

    rb.AddForce(launchDirection * dragDistance * launchForceMultiplier, ForceMode2D.Impulse);

    // 시각적 피드백 초기화
    if (lineRenderer != null)
    {
      lineRenderer.positionCount = 0;
    }
  }
}