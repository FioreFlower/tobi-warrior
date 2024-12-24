using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public AudioSource aSource;
    public AudioClip jumpSound;
    
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject damageField;
    [SerializeField] private GameObject trajectoryDotPrefab;
    [SerializeField] private float launchForceMultiplier = 3f;
    [SerializeField] private int maxDots = 10;

    private readonly List<GameObject> _trajectoryDots = new List<GameObject>(); // 활성화된 점 리스트
    private GameObject _instantDamageField;
    private Animator _animator;
    private Rigidbody2D _rb;
    private Camera _mainCamera;
    private Vector2 _startDragPosition;
    private Vector2 _endDragPosition;
    private bool _isDragging = false;
    private bool _isGrounded = true;
    private bool _canJump = true;
    
    private enum AnimationState { Idle, Jump, Fall, Attack, Croush }
    private static readonly int[] AnimationHashes = {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("jump"),
        Animator.StringToHash("Fall"),
        Animator.StringToHash("Attack"),
        Animator.StringToHash("Croush")
    };
    
    private void Awake()
    {
        aSource = FindObjectOfType<AudioSource>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if(!_canJump) return;
        _startDragPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _isDragging = true;
        PlayAnimation(AnimationState.Croush);
    }

    private void OnMouseDrag()
    {
        if (!_isDragging || !_canJump) return;

        Vector2 currentDragPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 launchDirection = (_startDragPosition - currentDragPosition).normalized;
        float dragDistance = Vector2.Distance(_startDragPosition, currentDragPosition);
        Vector2 launchVelocity = launchDirection * dragDistance * launchForceMultiplier;

        
        DrawVisualFeedback(currentDragPosition);
        DrawTrajectory(_startDragPosition, launchVelocity); // 궤적 표시
    }

    private void OnMouseUp()
    {
        if (!_isDragging) return;

        _endDragPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        _isDragging = false;
        _isGrounded = false;
        _canJump = false;
        
        PlayAnimation(AnimationState.Jump);
        Vector2 launchDirection = (_startDragPosition - _endDragPosition).normalized;
        float dragDistance = Vector2.Distance(_startDragPosition, _endDragPosition);
        _rb.AddForce(launchDirection * dragDistance * launchForceMultiplier, ForceMode2D.Impulse);

        aSource.PlayOneShot(jumpSound);
        StartCoroutine(DestroyAfterDelay());
        
        ClearVisualFeedback();
        ClearTrajectory();
        GameManager.Instance.SetActiveMiniMap(false);
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        
        // 공격 애니메이션이 실행 중이면 대기
        yield return new WaitUntil(() => !IsInAnimation(AnimationState.Attack));
        
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<EnemyController>()?.TakeDamage(50f);
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            _isGrounded = true;
            PlayAnimation(AnimationState.Attack);
            
        } else
        {
            if (IsInAnimation(AnimationState.Attack)) return;
            
            _isGrounded = true;
            PlayAnimation(AnimationState.Idle);
        }
    }
    
    private void Update()
    {
        if (!_isGrounded)
        {
            if (_rb.velocity.y > 0 && !IsInAnimation(AnimationState.Jump)) // 올라가는 중이면 Jump 애니메이션 재생 
            {
                PlayAnimation(AnimationState.Jump);
            }
            else if (_rb.velocity.y < 0 && !IsInAnimation(AnimationState.Fall)) // 떨어지기 시작하면 Fall 애니메이션 재생
            {
                PlayAnimation(AnimationState.Fall);
                _canJump = false;
            }
        }
    }
    
    private void DrawTrajectory(Vector2 startPoint, Vector2 launchVelocity)
    {
        Vector2 gravity = Physics2D.gravity;
        const float timeStep = 0.05f;
        for (int i = 0; i < maxDots; i++)
        {
            float t = i * timeStep;
            // 현재 점 위치 계산
            Vector2 currentPoint = startPoint + launchVelocity * t + 0.1f * gravity * t * t;
            
            // 점 생성 또는 재활용
            if (i >= _trajectoryDots.Count)
            {
                GameObject dot = Instantiate(trajectoryDotPrefab, currentPoint, Quaternion.identity);
                _trajectoryDots.Add(dot);
            }
            else
            {
                _trajectoryDots[i].transform.position = currentPoint;
                _trajectoryDots[i].SetActive(true);
            }
        }

        // 나머지 점 비활성화
        for (int i = _trajectoryDots.Count - 1; i >= maxDots; i--)
        {
            _trajectoryDots[i].SetActive(false);
        }
    }
    
    private void ClearTrajectory()
    {
        foreach (GameObject dot in _trajectoryDots)
        {
            dot.SetActive(false); // 점 비활성화
        }
    }
    
    private void DrawVisualFeedback(Vector2 currentDragPosition)
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, _startDragPosition);
            lineRenderer.SetPosition(1, currentDragPosition);
        }
    }
    
    private void ClearVisualFeedback()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }
    }

    private void PlayAnimation(AnimationState state)
    {
        _animator.Play(AnimationHashes[(int)state]);
    }
    
    private bool IsInAnimation(AnimationState state)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == AnimationHashes[(int)state];
    }

    private void Attack()
    {
        _instantDamageField = Instantiate(damageField, transform.position, Quaternion.identity);
    }

    private void AttackEnd()
    {
        Destroy(_instantDamageField);
    }

    private void OnDestroy()
    {
        GameManager.Instance.SetActiveMiniMap(true);
        Destroy(_instantDamageField);
    }
}
