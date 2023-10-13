using GameExtensions.Debug;
using JetBrains.Annotations;
using UnityEngine;

namespace GameExtensions
{
    public class StateManager : MonoBehaviour
    {
        public State CurrentState { get; private set; }
        
        public void SetState(State newState)
        {
            if (newState is null)
            {
                DebugConsole.LogError("Trying to set null state!");
                return;
            }
            CurrentState?.ExitState();
            CurrentState = newState;
            DebugConsole.Log("New state: " + CurrentState);
            CurrentState.Start();
        }

        #region EventFunctions

        protected void Update()
        {
            CurrentState.Update();
        }

        protected void FixedUpdate()
        {
            CurrentState.FixedUpdate();
        }

        protected void LateUpdate()
        {
            CurrentState.LateUpdate();
        }

        protected void OnDisable()
        {
            CurrentState.OnDisable();
        }

        protected void OnCollisionEnter(Collision other)
        {
            CurrentState.OnCollisionEnter(other);
        }

        protected void OnCollisionExit(Collision other)
        {
            CurrentState.OnCollisionExit(other);
        }

        protected void OnCollisionStay(Collision other)
        {
            CurrentState.OnCollisionStay(other);
        }

        protected void OnDrawGizmos()
        {
            CurrentState?.OnDrawGizmos();
        }

        protected void OnTriggerEnter(Collider other)
        {
            CurrentState.OnTriggerEnter(other);
        }

        protected void OnTriggerExit(Collider other)
        {
            CurrentState.OnTriggerExit(other);
        }

        protected void OnTriggerStay(Collider other)
        {
            CurrentState.OnTriggerStay(other);
            _ = other;
        }

        #endregion
    }
}