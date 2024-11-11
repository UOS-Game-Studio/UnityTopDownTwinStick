using Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace AI
{
    public class S_AIController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator anim;
        private GameObject PlayerRef;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            anim.applyRootMotion = false;
            agent.updatePosition = true;
            agent.updateRotation = true;

            PlayerRef = GameObject.Find("PlayerBase");

        }

        RaycastHit hit;
        private bool isDead = false;
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        void Update()
        {
            if (isDead)
                return;
            
            if (Vector3.Distance(PlayerRef.transform.position, this.transform.position) < agent.stoppingDistance)
            {
                anim.SetBool("CanAttack", true);
            }
            else
            {
                anim.SetBool("CanAttack", false);
                agent.destination = PlayerRef.transform.position;
                anim.SetFloat("VelocityX", agent.velocity.magnitude);

                //Vector3 mouseLocation = new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, 10);

                //Ray ray = Camera.main.ScreenPointToRay(
                //    mouseLocation);
                //if (Physics.Raycast(ray, out hit))
                //{
                //    agent.destination = hit.point;
                //    anim.SetFloat("VelocityX", agent.velocity.magnitude);
                //}
            }
        }
        void OnDrawGizmos()
        {
            //Gizmos.DrawCube(hit.point, new Vector3(0.5f, 0.5f,0.5f));
            //Gizmos.DrawWireSphere(this.transform.position, agent.stoppingDistance);
        }

        void OnHitPlayer()
        {
            Debug.Log("hit player");
        }

        public void OnDeath(Health sender)
        {
            isDead = true;
            anim.SetTrigger(IsDead);
            agent.velocity = Vector3.zero;
        }

        public void OnTakeDamage()
        {
            Debug.Log("zombie hit");
        }
        
    }

    
}
