using Components;
using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class ReleaseTrailSystem : IEcsRunSystem
    { 
        float minXPos;
        float maxXPos;
        float minYPos;
        float maxYPos;
        float minDuration;
        float maxDuration;

        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            
            var followPool = ecsWorld.GetPool<FollowComponent>();
            var headDestroyerPool = ecsWorld.GetPool<HeadDestroyerComponent>();
            var ownerComponentPool = ecsWorld.GetPool<OwnedComponent>();
            var transformRefPool = ecsWorld.GetPool<TransformRefComponent>();
            var releaseTrailTimerPool = ecsWorld.GetPool<TimerComponent>();
            
            foreach (var i in systems.GetWorld().Filter<ReleaseTrailComponent>()
                         .Inc<FollowComponent>()
                         .Inc<HeadDestroyerComponent>()
                         .Inc<OwnedComponent>()
                         .Inc<TransformRefComponent>().End())
            {
                followPool.Del(i);
                headDestroyerPool.Del(i);
                ownerComponentPool.Del(i);
                ref var releaseTrailTimerComponent = ref releaseTrailTimerPool.Add(i);
                var duration = Random.Range(minDuration, maxDuration);
                releaseTrailTimerComponent.timer = duration;
                ref var transformRef = ref transformRefPool.Get(i);
                transformRef.transform.DOMove(new Vector3(Random.Range(minXPos, maxXPos), Random.Range(minYPos, maxYPos)), duration);
            }
        }
        
        public ReleaseTrailSystem(float minXPos, float maxXPos, float minYPos, float maxYPos, float minDuration, float maxDuration)
        {
            this.minXPos = minXPos;
            this.maxXPos = maxXPos;
            this.minYPos = minYPos;
            this.maxYPos = maxYPos;
            this.minDuration = minDuration;
            this.maxDuration = maxDuration;
        }
    }
}