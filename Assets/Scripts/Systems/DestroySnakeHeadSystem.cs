using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class DestroySnakeHeadSystem : IEcsRunSystem
    {
        private readonly List<int> entityForDestroy = new List<int>();
        private readonly List<GameObject> gameobjectsForDestroy = new List<GameObject>();
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var collisionEventsPool = world.GetPool<CollisionEntitiesEventComponent>();
            var headDestroyerPool = world.GetPool<HeadDestroyerComponent>();
            var ownerPool = world.GetPool<OwnedComponent>();
            var releaseTrailPool = world.GetPool<ReleaseTrailComponent>();
            
            entityForDestroy.Clear();
            gameobjectsForDestroy.Clear();
            
            foreach (var i in world.Filter<CollisionEntitiesEventComponent>().End())
            {
                ref var eventComponent = ref collisionEventsPool.Get(i);

                var collidedEntity = eventComponent.other.Entity;
                
                if (!headDestroyerPool.Has(collidedEntity))
                    continue;
                
                if (headDestroyerPool.Get(collidedEntity).exclude == eventComponent.entity.Entity)
                    continue;
                
                Debug.Log(eventComponent.other + " destroyed " + collidedEntity);
                gameobjectsForDestroy.Add(eventComponent.gameObject);
                entityForDestroy.Add(eventComponent.entity.Entity);
            }

            foreach (var gameObject in gameobjectsForDestroy)
            {
                Object.Destroy(gameObject);
            }

            foreach (var entity in entityForDestroy)
            {
                world.DelEntity(entity);
            }
            
            foreach (var i in world.Filter<OwnedComponent>().End())
            {
                var owned = ownerPool.Get(i);
                if (entityForDestroy.Contains(owned.owner))
                    releaseTrailPool.Add(i);
            }
        }
    }
}