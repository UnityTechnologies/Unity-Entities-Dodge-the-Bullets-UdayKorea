using TMPro;
using Unity.Entities;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        var elapsedTime = World.DefaultGameObjectInjectionWorld.Time.ElapsedTime;
        _text.text = $"{elapsedTime:00.00}";
    }
}
