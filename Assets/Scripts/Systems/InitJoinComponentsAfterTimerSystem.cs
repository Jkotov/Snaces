using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class InitJoinComponentsAfterTimerSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var releaseTrainPool = world.GetPool<ReleaseTrailComponent>();
            var timerPool = world.GetPool<TimerComponent>();
            
            var joinPool = world.GetPool<JoinComponent>();
            var npcTargetPool = world.GetPool<NpcTargetComponent>();
            
            foreach (var i in world.Filter<ReleaseTrailComponent>().Inc<TimerComponent>().End())
            {
                ref var timer = ref timerPool.Get(i);
                if (timer.timer > 0)
                    continue;
                timerPool.Del(i);
                releaseTrainPool.Del(i);

                joinPool.Add(i);
                npcTargetPool.Add(i);
            }
        }
    }
}