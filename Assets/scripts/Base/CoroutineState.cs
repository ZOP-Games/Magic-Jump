using System.Collections;

namespace GameExtensions
{
    public abstract class CoroutineState : State
    {
        protected CoroutineState(StateManager context) : base(context)
        {
        }

        protected abstract IEnumerator Coroutine();

        public override void Start()
        {
            context.StartCoroutine(nameof(Coroutine));
        }

        public override void ExitState()
        {
            context.StopCoroutine(nameof(Coroutine));
        }
    }
}