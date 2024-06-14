using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using Systems;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartUp : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> peasants;
    public float peasantMaxSpeed = 7f;
    public float snackSpeed = 5f;
    public float agentsSize = 1f;
    public GameObject npcPrefab;
    public List<Transform> spawns;
    public int npcCount;
    
    [Header("Trail on destroy data")]
    public float minXPos = -10f;
    public float maxXPos = 10f;
    public float minYPos = -10f;
    public float maxYPos = 10f;
    public float minDuration = 1f;
    public float maxDuration = 2f;
    
    private EcsWorld _world;
    private IEcsSystems updateSystems;
    private IEcsSystems fixedUpdateSystems;

    void Start()
    {
        _world = new EcsWorld();
        updateSystems = new EcsSystems(_world);

        updateSystems
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
            .Add(new FollowSystem())
            .Add(new PlayerInputSystem())
            .Add(new TimerSystem())
            .Add(new InitJoinComponentsAfterTimerSystem())
            .Add(new CheckWinSystem())
            .Add(new CheckLoseSystem())
            .Add(new SendGameResultSystem())
            .Init();

        fixedUpdateSystems = new EcsSystems(_world);
        
        fixedUpdateSystems
            .Add(new JoinOnCollisionSystem())
            .Add(new DelJoinSystem())
            .Add(new PlayerDirectionSystem())
            .Add(new NpcDirectionSystem())
            .Add(new DestroySnakeHeadSystem())
            .Add(new ReleaseTrailSystem(minXPos, maxXPos,  minYPos, maxYPos, minDuration, maxDuration))
            .Add(new DelCollisionEventSystem())
            .Init();
        
        InitPeasants();
        CreateSnakes();
    }

    void Update()
    {
        updateSystems.Run();
    }

    private void FixedUpdate()
    {
        fixedUpdateSystems.Run();
    }

    void OnDestroy()
    {
        updateSystems.Destroy();
        _world.Destroy();
    }

    private void InitPeasants()
    {
        var positionPool = _world.GetPool<TransformRefComponent>();
        var joinPool = _world.GetPool<JoinComponent>();
        var speedPool = _world.GetPool<SpeedComponent>();
        var sizePool = _world.GetPool<SizeComponent>();
        var npcTargetPool = _world.GetPool<NpcTargetComponent>();
        
        foreach (var peasant in peasants)
        {
            var entity = _world.NewEntity();
            
            var packedEntity = peasant.GetComponent<PackedEntity>();
            packedEntity.Init(entity, _world);
            
            ref var positionComponent = ref positionPool.Add(entity);
            positionComponent.transform = peasant.transform;

            joinPool.Add(entity);
            
            ref var speedComponent = ref speedPool.Add(entity);
            speedComponent.speed = peasantMaxSpeed;

            ref var sizeComponent = ref sizePool.Add(entity);
            sizeComponent.size = agentsSize;
            
            npcTargetPool.Add(entity); 
        }
    }
    
    private void CreateSnakes()
    {
        AddPlayerComponents(InitSnake(snackSpeed, player));

        for (int i = 0; i < npcCount; i++)
        {
            var pos = spawns[Random.Range(0, spawns.Count)].position;
            // magic values for testing with many NPCs
            pos.x += Random.Range(-1f, 1f);
            pos.y += Random.Range(-1f, 1f);
            var head = Instantiate(npcPrefab, pos, Quaternion.identity);
            AddNpcComponents(InitSnake(snackSpeed, head));
        }
    }

    private void AddPlayerComponents(int entity)
    {
        _world.GetPool<PlayerControllerComponent>().Add(entity);
    }

    private void AddNpcComponents(int entity)
    {
        _world.GetPool<NpcControllerComponent>().Add(entity);
    }
    
    private int InitSnake(float speed, GameObject head)
    {
        var entity = _world.NewEntity();
        
        var positionPool = _world.GetPool<TransformRefComponent>();
        var velocityPool = _world.GetPool<Rb2DRefComponent>();
        var joinTargetPool = _world.GetPool<JoinTargetComponent>();
        
        ref var positionComponent = ref positionPool.Add(entity);
        positionComponent.transform = head.transform;

        ref var velocityComponent = ref velocityPool.Add(entity);
        velocityComponent.rb2D = head.GetComponent<Rigidbody2D>();

        ref var speedComponent = ref _world.GetPool<SpeedComponent>().Add(entity);
        speedComponent.speed = speed;
        
        ref var joinTargetComponent = ref joinTargetPool.Add(entity);
        joinTargetComponent.followTargetEntity = entity;
        
        ref var sizeComponent = ref _world.GetPool<SizeComponent>().Add(entity);
        sizeComponent.size = agentsSize;

        if (head.TryGetComponent(out PackedEntity packedEntity))
        {
            packedEntity.Init(entity, _world);
        }
        
        return entity;
    }
}

