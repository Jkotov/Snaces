using Leopotam.EcsLite;
using UnityEngine;

public class PackedEntity : MonoBehaviour
{
    public int Entity
    {
        get
        {
            packedEntity.Unpack(World, out var value);
            return value;
        }
    }
    public EcsWorld World { get; private set; }
    
    private EcsPackedEntity packedEntity;
    
    public void Init(int entity, EcsWorld world)
    {
        this.World = world;
        packedEntity = world.PackEntity(entity);
    }
}