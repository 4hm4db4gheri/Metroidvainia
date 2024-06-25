using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{

    [SerializeField] Transform[] checkPointsPositions;
    [SerializeField] float radius = 1f;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float fallingRespawnPointRange = 1f;
    private int currentCheckPointIndex = 0;
    public static RespawnSystem instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckLastCheckPoint();
    }

    private void CheckLastCheckPoint()
    {
        if (currentCheckPointIndex + 1 < checkPointsPositions.Length)
        {
            var hit = Physics2D.OverlapCircle(checkPointsPositions[currentCheckPointIndex + 1].position, radius, playerMask);
            if (hit)
                currentCheckPointIndex++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var item in checkPointsPositions)
        {
            Gizmos.DrawWireSphere(item.position, radius);
        }
    }

    public void Respawn(Character player)
    {
        Vector2 respawnPosition = checkPointsPositions[currentCheckPointIndex].position;
        respawnPosition.y += fallingRespawnPointRange;
        player.transform.position = respawnPosition;
    }
}
