using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public float BaseHeight;
    public float BobbleSpeed = 5;
    public float BobbleAmount = .25f;
    public float RotationSpeed = 5;

    public void Awake()
    {
        BaseHeight = transform.position.y;
    }

    public void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = BaseHeight + (BobbleAmount * Mathf.Sin(Time.time * BobbleSpeed));
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(0, Time.time*RotationSpeed, 0);
    }

}