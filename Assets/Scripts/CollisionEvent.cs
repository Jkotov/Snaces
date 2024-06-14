using Components;
using UnityEngine;

[RequireComponent(typeof(PackedEntity))]
public class CollisionEvent : MonoBehaviour
{
    public PackedEntity packedEntity;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PackedEntity>(out var otherEntity))
            return;
        var collisionEntity = packedEntity.World.NewEntity();
        var pool = packedEntity.World.GetPool<CollisionEntitiesEventComponent>();
        if (pool.Has(packedEntity.Entity))
            return;
        ref var component = ref pool.Add(collisionEntity);
        component.gameObject = gameObject;
        component.entity = packedEntity;
        component.other = otherEntity;
    }
}
