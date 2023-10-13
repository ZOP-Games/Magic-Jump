namespace GameExtensions.Enemies
{
    public class EnemyDeathState : EnemyState
    {
        private readonly int rewardXp;


        public EnemyDeathState(EnemyStateManager enemy,int reward) : base(enemy)
        {
            rewardXp = reward;
        }

        protected override void CheckForTransition()
        {
        }

        public override void Start()
        {
            base.Start();
            Player.Instance.AddXp(rewardXp);
            DontLookAtMe();
            UnityEngine.Object.Destroy(enemy.GetComponentInChildren<EnemyLocation>().gameObject);
        }
    }
}