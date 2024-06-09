using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerFolloingLight : MonoBehaviour
{
    [SerializeField] Light2D playerFollowingLigth;
    [SerializeField] Light2D globalLight;
    [SerializeField] float detectionRange;
    [SerializeField] LayerMask detectionLayer;


    private void Update()
    {
        var hit = Physics2D.OverlapCircle(transform.position, detectionRange, detectionLayer);
        if (hit)
        {
            playerFollowingLigth.enabled = true;
            globalLight.intensity = 0.005f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
