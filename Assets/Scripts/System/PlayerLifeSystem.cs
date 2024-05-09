using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerLifeSystem : ISystem
{
    private EntityQuery playerQuery;
    
    public void OnCreate(ref SystemState state)
    {
        // LocalTransform and Player
        playerQuery = state.EntityManager.CreateEntityQuery(
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<Player>());
        
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(playerQuery.CalculateEntityCount() == 0)
        {
            return;
        }
        
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        
        var playerEntities 
            = playerQuery.ToEntityArray(Allocator.TempJob);
        var playerTransforms 
            = playerQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
        
        // compare position with bullets and destroy player if they are close
        for (var i = 0; i < playerEntities.Length; i++)
        {
            var playerTransform = playerTransforms[i];
            var playerEntity = playerEntities[i];
            
            foreach (var bulletTransformRef 
                     in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Bullet>())
            {
                var distance 
                    = math.distance(playerTransform.Position, bulletTransformRef.ValueRO.Position);

                if (distance < 0.1)
                {
                    ecb.DestroyEntity(playerEntity);
                }
            }
        }
    }
}
