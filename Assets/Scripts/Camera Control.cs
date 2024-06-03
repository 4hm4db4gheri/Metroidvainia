using UnityEngine;

public class CameraControl : MonoBehaviour
{
   [SerializeField] private Vector2 offset;
   [SerializeField] private Transform target;
   [SerializeField] float cameraXGrid;
   [SerializeField] float cameraYGrid;

   private void Update()
   {
      // Vector3 pos = target.position;
      // pos.x += offset.x;
      // pos.y = offset.y;
      // pos.z = -10;
      // transform.position = pos;
      Vector3 pos = transform.position;
      if ((target.position.x - transform.position.x) > cameraXGrid / 2)
      {
         pos.x += target.position.x - transform.position.x - cameraXGrid / 2;
         transform.position = pos;
      }
      else if ((target.position.x - transform.position.x) < -cameraXGrid / 2)
      {
         pos.x += target.position.x - transform.position.x + cameraXGrid / 2;
         transform.position = pos;
      }

   }
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(transform.position, new Vector2(cameraXGrid, cameraYGrid));
   }
}





