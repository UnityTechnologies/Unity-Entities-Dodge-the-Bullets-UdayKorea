using Unity.Entities;

public struct GameState : IComponentData
{
    public bool IsGameRunning; // 게임이 시작되어 진행중인가
    public bool IsGameOver; // 게임 오버 상태
    public int PlayerCount; // 살아있는 플레이어 수
}