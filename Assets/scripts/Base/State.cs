using UnityEngine;

namespace GameExtensions
{
    public abstract class State
    {
        protected readonly StateManager context;

        protected abstract void CheckForTransition();

        #region EventFunctions

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void OnCollisionEnter(Collision collision)
        {

        }

        public virtual void OnCollisionStay(Collision collision)
        {

        }

        public virtual void OnCollisionExit(Collision collision)
        {

        }

        public virtual void OnTriggerEnter(Collider collider)
        {

        }

        public virtual void OnTriggerStay(Collider collider)
        {

        }

        public virtual void OnTriggerExit(Collider collider)
        {

        }

        public virtual void OnDrawGizmos()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void ExitState()
        {

        }

        #endregion

        protected State(StateManager context)
        {
            this.context = context;
        }
    }
}