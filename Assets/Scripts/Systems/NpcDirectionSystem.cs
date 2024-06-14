using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class NpcDirectionSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var rb2DRefPool = world.GetPool<Rb2DRefComponent>();
            var speedPool = world.GetPool<SpeedComponent>();
            
            foreach (var i in world.Filter<NpcControllerComponent>().End())
            {
                ref var rb2DRef = ref rb2DRefPool.Get(i);
                var rb2DPosition = rb2DRef.rb2D.position;
                
                if (!Utils.TryGetNearestEntityWithFilterPosition(world, world.Filter<NpcTargetComponent>().End(),
                        rb2DPosition, out var nearestEntityPosition)) continue;
                
                ref var speed = ref speedPool.Get(i);
                var direction = nearestEntityPosition - rb2DPosition;
                rb2DRef.rb2D.velocity = direction.normalized * speed.speed;
            }
        }
    }

}