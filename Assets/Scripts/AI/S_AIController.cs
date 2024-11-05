using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace AI
{
    public class S_AIController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator anim;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            anim.applyRootMotion = false;
            agent.updatePosition = true;
            agent.updateRotation = true;
        }

        RaycastHit hit;
        void Update()
        {
            Vector3 mouseLocation = new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, 10);

            
            Ray ray = Camera.main.ScreenPointToRay(
                    mouseLocation);
            if (Physics.Raycast(ray, out hit))
            {
                

                //Vector3 MouseNoY = new Vector3(agent.destination.x, 0, agent.destination.z);
                //Vector3 porNoY = new Vector3(transform.position.x, 0, transform.position.z);
                
                //Debug.Log("-----------------");
                //Debug.Log("MouseNoY: " +MouseNoY);
                //Debug.Log("porNoY: " + porNoY);
                
                
                agent.destination = hit.point;
               // Vector3 AgentToMouseDir = (MouseNoY - porNoY).normalized;
                anim.SetFloat("VelocityX", agent.velocity.magnitude);
                
                //Debug.Log("AgentToMouseDir.magnitude: " + AgentToMouseDir.magnitude);
            }

        }
        void OnDrawGizmos()
        {
            Gizmos.DrawCube(hit.point, new Vector3(0.5f, 0.5f,0.5f));
        }
    }

    
}
