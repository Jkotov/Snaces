using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class TimerSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var ecsPool = world.GetPool<TimerComponent>();
            
            foreach (var i in world.Filter<TimerComponent>().End())
            {
                ecsPool.Get(i).timer -= Time.deltaTime;
            }
        }
    }
}