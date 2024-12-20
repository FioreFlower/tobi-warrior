using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    
    [SerializeField] private Transform turret;
    private float minJumpDegree = 10;
    private float maxJumpDegree = 80;

    
    [SerializeField] private float turretRotationSpeed = 10;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }
    
}
