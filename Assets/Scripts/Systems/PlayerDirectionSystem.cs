using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class PlayerDirectionSystem : IEcsRunSystem
    {
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var playerControllerPool = world.GetPool<PlayerControllerComponent>();
            var rb2DRefPool = world.GetPool<Rb2DRefComponent>();
            var speedPool = world.GetPool<SpeedComponent>();
            foreach (var i in world.Filter<PlayerControllerComponent>().Inc<SpeedComponent>().End())
            {
                ref var playerControllerComponent = ref playerControllerPool.Get(i);
                ref var rb2DRefComponent = ref rb2DRefPool.Get(i);
                ref var speedComponent = ref speedPool.Get(i);
                
                Vector3 dir;
                
                if (playerControllerComponent.input != Vector3.zero)
                {
                    dir = playerControllerComponent.input.normalized;
                }
                else
                {
                    dir = rb2DRefComponent.rb2D.velocity.normalized;
                }

                if (dir == Vector3.zero)
                {
                    dir = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f)).normalized;
                }

                rb2DRefComponent.rb2D.velocity = dir * speedComponent.speed;
            }
        }
    }
}