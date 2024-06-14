using Components;
using Leopotam.EcsLite;
using UnityEngine;

public static class Utils
{
    public static bool TryGetNearestEntityWithFilter(EcsWorld world, EcsFilter filter, Vector2 centerPos, out int entity)
    {
        var pool = world.GetPool<TransformRefComponent>();
        float minSqDistance = float.PositiveInfinity;
        entity = -1;
        foreach (var i in filter)
        {
            var entityPos = pool.Get(i).transform.position;
            var sqrMagnitude = (new Vector2(entityPos.x, entityPos.y) - centerPos).sqrMagnitude;
            if (sqrMagnitude < minSqDistance)
            {
                minSqDistance = sqrMagnitude;
                entity = i;
            }
        }
        return minSqDistance < float.PositiveInfinity;
    }

    public static bool TryGetNearestEntityWithFilterPosition(EcsWorld world, EcsFilter filter, Vector2 centerPos,
        out Vector2 position)
    {
        
        var pool = world.GetPool<TransformRefComponent>();
        float minSqDistance = float.PositiveInfinity;
        position = Vector2.zero;
        foreach (var i in filter)
        {
            var entityPos = pool.Get(i).transform.position;
            var pos2D = new Vector2(entityPos.x, entityPos.y);
            var sqrMagnitude = (pos2D - centerPos).sqrMagnitude;
            if (sqrMagnitude < minSqDistance)
            {
                minSqDistance = sqrMagnitude;
                position = pos2D;
            }
        }

        return minSqDistance < float.PositiveInfinity;
    }
}