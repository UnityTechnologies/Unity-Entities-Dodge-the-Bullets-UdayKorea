using TMPro;
using Unity.Entities;
using UnityEngine;

public class GameoverText : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        _text.enabled = false;
        // use GetExistingSystemManaged to get the system instance
        var gameManagerSystem =
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameManagerSystem>();
        gameManagerSystem.OnGameOver += () => _text.enabled = true;
    }
}
