using Components;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Networking;

namespace Systems
{
    public class SendGameResultSystem : IEcsRunSystem
    {
        private const string Http = "http://example.com/";
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var gameOverComponentPool = world.GetPool<GameOverComponent>();
            
            foreach (var i in world.Filter<GameOverComponent>().End())
            {
                ref var gameOverComponent = ref gameOverComponentPool.Get(i);
                if (gameOverComponent.isResultSent)
                    continue;

                var form = new WWWForm();
                form.AddField("Result", gameOverComponent.result);
                UnityWebRequest.Post(Http, form);
                gameOverComponent.isResultSent = true;
            }
        }
    }
}