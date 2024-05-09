using Unity.Entities;
using UnityEngine;

// Game State 게임 오브젝트를 엔티티로 변환
public class GameStateAuthoring : MonoBehaviour
{
    private class Baker : Baker<GameStateAuthoring>
    {
        public override void Bake(GameStateAuthoring authoring)
        {
            // 움직일 필요가 없으므로 TransformUsageFlags.None 플래그를 사용
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<GameState>(entity);
        }
    }
}