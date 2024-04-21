using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
   [SerializeField] private Vector2 offset;
   [SerializeField] private Transform target;

   private void Update()
   {
      Vector3 pos = target.position;
      pos.x += offset.x;
      pos.y = offset.y;
      pos.z = -10;
      transform.position = pos;
   }
}
