using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class CheckWinSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            if (world.Filter<GameOverComponent>().End().GetEntitiesCount() > 0)
                return;

            if (world.Filter<NpcControllerComponent>().End().GetEntitiesCount() > 0)
                return;

            Debug.Log("Win");
            ref var gameOverComponent = ref world.GetPool<GameOverComponent>().Add(world.NewEntity());
            gameOverComponent.result = "Win";
            gameOverComponent.isResultSent = false;
        }
    }
}