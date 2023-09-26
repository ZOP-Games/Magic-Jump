using UnityEngine;

namespace GameExtensions
{
    public class StateManager : MonoBehaviour
    {
        public State CurrentState { get; private set; }
        
        public void SetState(State newState)
        {
            CurrentState?.ExitState();
            CurrentState = newState;
            CurrentState.Start();
        }

        #region EventFunctions

        private void Update()
        {
            CurrentState.Update();
        }

        private void FixedUpdate()
        {
            CurrentState.FixedUpdate();
        }

        private void LateUpdate()
        {
            CurrentState.LateUpdate();
        }

        private void OnDestroy()
        {
            CurrentState?.OnDestroy();
        }

        private void OnCollisionEnter(Collision other)
        {
            CurrentState.OnCollisionEnter(other);
        }

        private void OnCollisionExit(Collision other)
        {
            CurrentState.OnCollisionExit(other);
        }

        private void OnCollisionStay(Collision other)
        {
            CurrentState.OnCollisionStay(other);
        }

        private void OnDrawGizmos()
        {
            CurrentState?.OnDrawGizmos();
        }

        private void OnTriggerEnter(Collider other)
        {
            CurrentState.OnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            CurrentState.OnTriggerExit(other);
        }

        private void OnTriggerStay(Collider other)
        {
            CurrentState.OnTriggerStay(other);
            _ = other;
        }

        #endregion
    }
}