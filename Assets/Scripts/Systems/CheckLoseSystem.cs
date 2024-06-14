using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class CheckLoseSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            if (world.Filter<GameOverComponent>().End().GetEntitiesCount() > 0)
                return;

            if (world.Filter<PlayerControllerComponent>().End().GetEntitiesCount() > 0)
                return;

            Debug.Log("Lose");
            ref var gameOverComponent = ref world.GetPool<GameOverComponent>().Add(world.NewEntity());
            gameOverComponent.result = "Lose";
            gameOverComponent.isResultSent = false;
        }
    }
}