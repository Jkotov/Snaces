using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class JoinOnCollisionSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var collisionPool = ecsWorld.GetPool<CollisionEntitiesEventComponent>();
            var joinTargetPool = ecsWorld.GetPool<JoinTargetComponent>();
            var sizePool = ecsWorld.GetPool<SizeComponent>();
            
            var followPool = ecsWorld.GetPool<FollowComponent>();
            var headDestroyerPool = ecsWorld.GetPool<HeadDestroyerComponent>();
            var ownedComponentPool = ecsWorld.GetPool<OwnedComponent>();

            var joinPool = ecsWorld.GetPool<JoinComponent>();
            var delJoinPool = ecsWorld.GetPool<DelJoinComponent>();
            
            foreach (var i in ecsWorld.Filter<CollisionEntitiesEventComponent>().End())
            {
                ref var component = ref collisionPool.Get(i);
                
                var entity = component.entity.Entity;
                var otherEntity = component.other.Entity;
                
                if (!joinPool.Has(entity)) continue;
                
                if (!joinTargetPool.Has(otherEntity)) continue;
                
                if (delJoinPool.Has(entity)) continue;
                
                ref var followComponent = ref followPool.Add(entity);
                var distance = sizePool.Get(entity).size / 2 + sizePool.Get(otherEntity).size / 2;
                followComponent.followSqDistance = distance * distance;
                ref var joinTargetComponent = ref joinTargetPool.Get(otherEntity);
                followComponent.targetEntity = joinTargetComponent.followTargetEntity;
                joinTargetComponent.followTargetEntity = entity;
                    
                ref var headDestroyerComponent = ref headDestroyerPool.Add(entity);
                headDestroyerComponent.exclude = otherEntity;
                    
                ref var ownedComponent = ref ownedComponentPool.Add(entity);
                ownedComponent.owner = otherEntity;

                delJoinPool.Add(entity);
            }
        }
    }
}