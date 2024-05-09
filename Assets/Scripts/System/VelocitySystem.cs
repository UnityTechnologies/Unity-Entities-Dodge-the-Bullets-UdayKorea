using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct VelocitySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (movementRef, localTransformRef)
                 in SystemAPI.Query<RefRO<Movement>, RefRW<LocalTransform>>())
        {
            var movement = movementRef.ValueRO.Velocity * SystemAPI.Time.DeltaTime;
            var newPosition = localTransformRef.ValueRO.Position + movement;
            localTransformRef.ValueRW.Position = newPosition;
        }
    }
}
