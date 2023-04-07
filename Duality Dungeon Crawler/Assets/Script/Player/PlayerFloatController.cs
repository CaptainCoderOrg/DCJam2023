using UnityEngine;
using CaptainCoder.Core;
using System;

public class PlayerFloatController : MonoBehaviour
{
    public static PlayerFloatController Instance;
    public float BobSpeed = 1f;
    public float BobRange = 1f;
    public float OffsetY = 2f;

    private float _baseY;
    private bool IsFloating => GameManager.Instance.Player.Effects.HasFlag(PlayerEffect.Float);

    public void Awake()
    {
        Instance = this;
        _baseY = transform.position.y;
    }
    
    public void Update()
    {
        Vector3 position = transform.position;
        position.y = !IsFloating ? _baseY : (Mathf.Sin(Time.time * BobSpeed)* BobRange) + OffsetY;
        transform.position = position;
    }

}