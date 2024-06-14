using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class FollowSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var positionPool = world.GetPool<TransformRefComponent>();
            var followPool = world.GetPool<FollowComponent>();
            var speedPool = world.GetPool<SpeedComponent>();
                
            foreach (var entity in world.Filter<TransformRefComponent>().Inc<FollowComponent>().End())
            {
                ref var positionComponent = ref positionPool.Get(entity);
                ref var followComponent = ref followPool.Get(entity);
                ref var speedComponent = ref speedPool.Get(entity);

                ref var targetPositionComponent = ref positionPool.Get(followComponent.targetEntity);

                var direction = targetPositionComponent.transform.position 
                                - positionComponent.transform.position;
                if (direction.sqrMagnitude > followComponent.followSqDistance)
                {
                    positionComponent.transform.position += direction.normalized * speedComponent.speed * Time.deltaTime;
                }
            }
        }
    }
}