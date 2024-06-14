using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class DelJoinSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var joinPool = ecsWorld.GetPool<JoinComponent>();
            var npcTargetPool = ecsWorld.GetPool<NpcTargetComponent>();
            var delJoinPool = ecsWorld.GetPool<DelJoinComponent>();
            
            foreach (var i in systems.GetWorld().Filter<DelJoinComponent>().End())
            {
                joinPool.Del(i);
                npcTargetPool.Del(i);
                delJoinPool.Del(i);
            }
        }
    }
}