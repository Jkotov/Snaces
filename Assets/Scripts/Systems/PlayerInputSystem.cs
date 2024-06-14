using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var pool = systems.GetWorld().GetPool<PlayerControllerComponent>();

            var input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            foreach (var i in systems.GetWorld().Filter<PlayerControllerComponent>().End())
            {
                pool.Get(i).input = input;
            }
        }
    }
}