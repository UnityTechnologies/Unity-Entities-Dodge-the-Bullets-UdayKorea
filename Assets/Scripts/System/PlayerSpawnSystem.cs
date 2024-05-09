using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct PlayerSpawnSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerSpawner>();
    }

    public void OnUpdate(ref SystemState state)
    {
    }

    public void OnStartRunning(ref SystemState state)
    {
        var playerSpawner = SystemAPI.GetSingleton<PlayerSpawner>();
        
        var rowCount = playerSpawner.PlayerCountInRow;
        var colCount = playerSpawner.PlayerCountInColumn;
        var offset = playerSpawner.Offset;

        var startPosition = new float3(0, 0, 0);
        
        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < colCount; j++)
            {
                var playerEntity = state.EntityManager.Instantiate(playerSpawner.PlayerPrefab);
                var localTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
                localTransform.Position = startPosition + new float3(i * offset, 0, j * offset);
                state.EntityManager.SetComponentData(playerEntity, localTransform);
            }
        }
    }

    public void OnStopRunning(ref SystemState state)
    {
        
    }
}
