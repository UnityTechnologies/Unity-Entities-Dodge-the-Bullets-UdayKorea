using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[BurstCompile]
[UpdateBefore(typeof(TransformSystemGroup))] // TransformSystemGroup 이전에 실행하여 총알 위치가 반영 안되는 문제 해결
public partial struct BulletSpawnSystem : ISystemStartStop, ISystem
{
    private Random _random; // 랜덤값 생성기

    // OnCreate는 시스템이 생성될 때 한번만 실행된다.
    public void OnCreate(ref SystemState state)
    {
        _random = new Random(999);
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<BulletSpawner>();
    }

    public void OnStartRunning(ref SystemState state)
    {
        Debug.Log("BulletSpawnSystem OnStartRunning()");
        var spawner = SystemAPI.GetSingleton<BulletSpawner>();
        
        for (var i = 0; i < spawner.PoolSize; i++)
        {
            var bulletEntity = state.EntityManager.Instantiate(spawner.BulletPrefab);
            state.EntityManager.AddComponent<Disabled>(bulletEntity);
            state.EntityManager.AddComponent<DisableRendering>(bulletEntity);
        }
    }

    public void OnStopRunning(ref SystemState state)
    {
        // do nothing
    }

    [BurstCompile]
    private float3 GetRandomPosition(float radius)
    {
        var randomPosFloat2 = _random.NextFloat2Direction() * radius;
        return new float3(randomPosFloat2.x, 0, randomPosFloat2.y);
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spawner = SystemAPI.GetSingleton<BulletSpawner>();
        
        if(spawner.NextSpawnTime > SystemAPI.Time.ElapsedTime)
        {
            return;
        }

        var aimPoint = float3.zero;

        foreach (var localTransformRef 
                 in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>())
        {
            aimPoint = localTransformRef.ValueRO.Position;
            break;
        }
        
        
        var bulletQuery = SystemAPI.QueryBuilder().WithAll<Bullet, Disabled>().Build();

        if (bulletQuery.CalculateEntityCount() < spawner.CountPerEachSpawn)
        {
            return;
        }
        
        var bulletEntities 
            = bulletQuery.ToEntityArray(state.WorldUpdateAllocator);
        for (var i = 0; i < spawner.CountPerEachSpawn; i++)
        {
            var bulletEntity = bulletEntities[i];
            state.EntityManager.RemoveComponent<Disabled>(bulletEntity);
            state.EntityManager.RemoveComponent<DisableRendering>(bulletEntity);

            var bulletPosition = GetRandomPosition(spawner.SpawnPositionRadius);
            var velocity
                = math.normalizesafe(aimPoint - bulletPosition)
                  * _random.NextFloat(spawner.BulletSpeedMinMax.x, spawner.BulletSpeedMinMax.y);

            var localTransform = LocalTransform.FromPositionRotationScale(bulletPosition, quaternion.identity, 0.2f);

            state.EntityManager.SetComponentData(bulletEntities[i], localTransform);
            state.EntityManager.SetComponentData(bulletEntities[i], new Movement
            {
                Velocity = velocity
            });
        }
        
        spawner.NextSpawnTime = SystemAPI.Time.ElapsedTime + spawner.TimeBetSpawn;
        SystemAPI.SetSingleton(spawner);
    }
}