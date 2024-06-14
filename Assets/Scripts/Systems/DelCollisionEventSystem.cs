using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class DelCollisionEventSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var collisionEventPool = world.GetPool<CollisionEntitiesEventComponent>();
            
            foreach (var i in world.Filter<CollisionEntitiesEventComponent>().End())
            {
                collisionEventPool.Del(i);
            }
        }
    }
}