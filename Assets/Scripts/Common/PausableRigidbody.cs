using System;
using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(Rigidbody))]
    public class PausableRigidbody : MonoBehaviour
    {
        private Vector3 _storedVelocity;
        private Rigidbody _rb;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
            PauseControl.OnPause.AddListener(PauseHandler);
        }

        private void PauseHandler(bool isPaused)
        {
            if (isPaused)
            {
                _storedVelocity = _rb.linearVelocity;
                _rb.linearVelocity = Vector3.zero;
                _rb.isKinematic = true;
            }
            else
            {
                _rb.isKinematic = false;
                _rb.linearVelocity = _storedVelocity;
            }
        }
        
    }
}