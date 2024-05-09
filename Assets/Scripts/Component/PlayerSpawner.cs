using Unity.Entities;

public struct PlayerSpawner : IComponentData
{
    public Entity PlayerPrefab;
    public int PlayerCountInRow;
    public int PlayerCountInColumn;
    public float Offset;
}