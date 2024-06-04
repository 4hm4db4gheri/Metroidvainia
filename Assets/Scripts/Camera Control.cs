using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
   [SerializeField] private Vector2 offset;
   [SerializeField] private Transform target;
   [SerializeField] float cameraXFrontGrid;
   [SerializeField] float cameraXBehindGrid;
   [SerializeField] float cameraYGrid;

   // private void Awake()
   // {
   //    Vector3 pos = target.position;
   //    pos.x += offset.x;
   //    pos.y = offset.y;
   //    pos.z = -10;
   //    transform.position = pos;
   // }

   private void Update()
   {
      Vector3 pos = transform.position;
      if ((target.position.x - transform.position.x) > cameraXFrontGrid)
      {
         pos.x += target.position.x - transform.position.x - cameraXFrontGrid;
         // transform.position = pos;
      }
      else if ((target.position.x - transform.position.x) < -cameraXBehindGrid)
      {
         pos.x += target.position.x - transform.position.x + cameraXBehindGrid;
         // transform.position = pos;
      }
      pos.y = target.position.y + offset.y;
      transform.position = pos;
      //  else if ((target.position.y - transform.position.y) > cameraYGrid / 2)
      // {
      //    pos.y += target.position.y - transform.position.y - cameraYGrid / 2;
      //    transform.position = pos;
      // }
      // else if ((target.position.y - transform.position.y) < -cameraYGrid / 2)
      // {
      //    pos.y += target.position.y - transform.position.y + cameraYGrid / 2;
      //    transform.position = pos;
      // }
      

   }
   // private void OnDrawGizmos()
   // {
   //    Gizmos.color = Color.red;
   //    Gizmos.DrawWireCube(transform.position, new Vector2(cameraXFrontGrid, cameraYGrid));
   // }
   private void OnDrawGizmos() {

      Gizmos.color = Color.red;
      
      Vector3 posFront = transform.position;
      posFront.x += cameraXFrontGrid;
      posFront.y += 2;
      Gizmos.DrawLine(posFront, new Vector2(posFront.x, posFront.y-6));

      Vector3 posBehind = transform.position;
      posBehind.x -= cameraXBehindGrid;
      posBehind.y += 2;
      Gizmos.DrawLine(posBehind, new Vector2(posBehind.x, posBehind.y-6));
   }
}





