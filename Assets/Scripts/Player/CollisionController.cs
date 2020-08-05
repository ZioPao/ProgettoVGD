using UnityEngine;

namespace Player
{
    public class CollisionController : MonoBehaviour
    {
        
        public void CheckCollisions()
        {
            //check ground

            Values.SetIsGrounded
            (
                Physics.Raycast(Values.GetRigidbody().transform.position, Vector3.down, out RaycastHit rayGround, 2)
            );     //todo determina l'altezza corretta

            LayerMask tmp = ~ LayerMask.GetMask("Player", "EnemyHitbox", "Ignore Raycast", "ViewCheckDefault", "tmpEnemy"); //ignore viewchecks for sprite management
            
            //todo aggiungi check per dietro al player
            //todo è totalmente rotto!!!!

            var maxDistance = 0.6f;
            Values.SetIsTouchingWall
            (
                (Physics.Raycast(Values.GetCollider().transform.position, Values.GetCollider().transform.forward, out _, maxDistance, tmp)
                 || Physics.Raycast(Values.GetCollider().transform.position, Values.GetCollider().transform.right, out _, maxDistance, tmp)
                 || Physics.Raycast(Values.GetCollider().transform.position, -Values.GetCollider().transform.forward, out _, maxDistance, tmp)
                 || Physics.Raycast(Values.GetCollider().transform.position, -Values.GetCollider().transform.right, out _, maxDistance, tmp)
                 
                 )
            );


            // if (Values.GetIsTouchingWall() == true)
            // {
            //     print("za warudo");
            // }
            // //Debug stuff
            //  var coll = Values.GetCollider();
            //  RaycastHit hit1, hit2, hit3, hit4;
            //  var t = (Physics.Raycast(coll.transform.position, coll.transform.forward,
            //               out hit1, maxDistance, tmp)
            //           || Physics.Raycast(Values.GetCollider().transform.position, Values.GetCollider().transform.right,
            //               out hit2, maxDistance, tmp)
            //           || Physics.Raycast(Values.GetCollider().transform.position,
            //               -Values.GetCollider().transform.forward, out hit3, maxDistance, tmp)
            //           || Physics.Raycast(Values.GetCollider().transform.position, -Values.GetCollider().transform.right,
            //               out hit4, maxDistance, tmp)
            //
            //      );
            //
            
            // //DEBUG
            // Debug.DrawRay(Values.GetCollider().transform.position,
            //     Values.GetCollider().transform.forward);
            // Debug.DrawRay(Values.GetCollider().transform.position,
            //     -Values.GetCollider().transform.forward);
            // Debug.DrawRay(Values.GetCollider().transform.position,
            //     Values.GetCollider().transform.right);
            // Debug.DrawRay(Values.GetCollider().transform.position,
            //     -Values.GetCollider().transform.right);
            //
            
            LayerMask layerTmp =~ LayerMask.GetMask("Player", "Ignore Raycast");

            if (Values.GetIsTouchingWallWithHead())
            {
                Values.SetIsTouchingWallWithHead
                (
                    Physics.Raycast(Values.GetCollider().transform.position, Values.GetCollider().transform.up, out var ray, 2.5f, layerTmp)
                );
            }
            else
            {
                Values.SetIsTouchingWallWithHead
                (
                    Physics.Raycast(Values.GetCollider().transform.position, Values.GetCollider().transform.up, out var ray, 2.5f,layerTmp) //ignore viewchecks for sprite management
                );
                
                if (Values.GetIsTouchingWallWithHead())
                {
                    Values.SetLastGoodYPosition(Values.GetRigidbody().position.y);
                }
            }
        }
        
        private void OnTriggerEnter(Collider c)
        {

            if (c.gameObject.CompareTag("Water"))
                Values.SetIsInWater(true);

        }

        private void OnTriggerExit(Collider c)
        {
            if (c.gameObject.CompareTag("Water"))
                Values.SetIsInWater(false);
        }
        
    }
    
}
