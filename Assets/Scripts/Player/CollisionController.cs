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

            LayerMask tmp = ~ LayerMask.GetMask("Enemy"); //ignore viewchecks for sprite management
            
            Values.SetIsTouchingWall
            (
                (Physics.Raycast(Values.GetCollider().transform.position + new Vector3(0, 0, Values.GetRaycastSpread()), Values.GetCollider().transform.forward, out _, 2, tmp)
                 || Physics.Raycast(Values.GetCollider().transform.position - new Vector3(0, 0, Values.GetRaycastSpread()), Values.GetCollider().transform.forward, out _, 2, tmp)
                 || Physics.Raycast(Values.GetCollider().transform.position, Values.GetCollider().transform.forward, out _, 2, tmp))
            );

            LayerMask layerTmp = ~ LayerMask.GetMask("Player");

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
