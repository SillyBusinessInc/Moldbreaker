namespace EnemiesNS
{
    public class BaseChasingState : StateBase
    {
        public BaseChasingState(EnemyBase enemy) : base(enemy) { }

        public override void Enter()
        {
            base.Enter();
            if (enemy.particleSystemWalk) enemy.particleSystemWalk.Play();
            enemy.isChasing = true;
            enemy.agent.speed = enemy.chaseSpeed;
            enemy.agent.acceleration = enemy.chaseAcceleration;
        }

        public override void Exit()
        {
            if (enemy.particleSystemWalk) enemy.particleSystemWalk.Stop();
            enemy.isChasing = false;
            base.Exit();
        }

        public override void Update()
        {
            if (enemy.agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete)
            {
                TransitionToPathBlockedRoaming();
                return;
            }

            if (!enemy.isWaiting)
            {
                if (enemy.distanceToPlayer <= enemy.minDistanceToPlayer)
                {
                    enemy.toggleIsWaiting(true);
                    enemy.FreezeMovement(true);
                    return;
                }
                if (enemy.agent.isStopped) enemy.FreezeMovement(false);
                enemy.agent.SetDestination(enemy.target.transform.position);
            }
            base.Update();
        }
    }
}