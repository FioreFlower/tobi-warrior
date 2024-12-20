using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private GameObject damageField;
    [SerializeField] private Transform turret;
    private bool isGrounded = true;
    private bool jumpCount = true;
    
    private Vector2 startDragPosition;
    private Vector2 endDragPosition;
    
    [SerializeField] private float launchForceMultiplier = 1f;
    [SerializeField] private LineRenderer lineRenderer;
    
    [SerializeField] private GameObject trajectoryDotPrefab; // 궤적 점으로 사용할 원 프리팹
    [SerializeField] private int maxDots = 15;              // 최대 점 개수
    private List<GameObject> trajectoryDots = new List<GameObject>(); // 생성된 점 리스트
    
    private bool isDragging = false;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        startDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
        Croush();
    }
    
    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector2 currentDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 launchDirection = (startDragPosition - currentDragPosition).normalized;
        float dragDistance = Vector2.Distance(startDragPosition, currentDragPosition);
        Vector2 launchVelocity = launchDirection * dragDistance * launchForceMultiplier;

        
        // 시각적 피드백 표시
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startDragPosition);
            lineRenderer.SetPosition(1, currentDragPosition);
        }
        // 궤적 표시
        DrawTrajectory(startDragPosition, launchVelocity);
    }
    
    void OnMouseUp()
    {
        if (!isDragging) return;

        endDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        isDragging = false;
        isGrounded = false;
        
        animator.Play("jump");
        Vector2 launchDirection = (startDragPosition - endDragPosition).normalized;
        float dragDistance = Vector2.Distance(startDragPosition, endDragPosition);

        rb.AddForce(launchDirection * dragDistance * launchForceMultiplier, ForceMode2D.Impulse);

        // 시각적 피드백 초기화
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }
        
        ClearTrajectory();
    }
    
    public void Croush()
    {
        if (jumpCount)
            animator.Play("Croush");
    }
    public void Idle() {
        animator.Play("Idle");
    }
    
    void Attack()
    {
        damageField.SetActive(true);
    }

    void AttackEnd()
    {
        damageField.SetActive(false);
    }
    
    private bool IsInAnimation(string animationName)
    {
        // 현재 애니메이터의 상태 정보 가져오기
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // 공격 애니메이션 상태인지 확인
        return currentState.IsName(animationName);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isGrounded = true;
            animator.Play("Attack");
        } else
        {
            if (IsInAnimation("Attack")) return;
            isGrounded = true;
            Idle();
        }
    }

    void JumpToFall()
    {
        if(!jumpCount) return;
        animator.Play("Fall");
        jumpCount = false;
    }
    
    private void ClearTrajectory()
    {
        foreach (GameObject dot in trajectoryDots)
        {
            dot.SetActive(false);
        }
    }
    
    private void DrawTrajectory(Vector2 startPoint, Vector2 launchVelocity)
    {
        Vector2 gravity = Physics2D.gravity;
        float timeStep = 0.01f;
        float totalDistance = 0f;

        Vector2 previousPoint = startPoint;

        for (int i = 0; i < maxDots; i++)
        {
            float t = i * timeStep;

            // 현재 점 위치 계산
            Vector2 currentPoint = startPoint + launchVelocity * t + 0.1f * gravity * t * t;
            
            // 점 생성 또는 재활용
            if (i >= trajectoryDots.Count)
            {
                GameObject dot = Instantiate(trajectoryDotPrefab, currentPoint, Quaternion.identity);
                trajectoryDots.Add(dot);
            }
            else
            {
                trajectoryDots[i].transform.position = currentPoint;
                trajectoryDots[i].SetActive(true);
            }

            previousPoint = currentPoint;
        }

        // 나머지 점 비활성화
        for (int i = trajectoryDots.Count - 1; i >= maxDots; i--)
        {
            trajectoryDots[i].SetActive(false);
        }
    }

    void Update()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y > 0) // 올라가는 중이면 Jump 애니메이션 재생
            {
                if (!IsInAnimation("jump"))
                {
                    animator.Play("jump");
                }
            }
            else if (rb.velocity.y < 0) // 떨어지기 시작하면 JumptoFall 애니메이션 재생
            {
                if (!IsInAnimation("Fall"))
                {
                    JumpToFall();
                    
                }
            }
        }
    }
}
