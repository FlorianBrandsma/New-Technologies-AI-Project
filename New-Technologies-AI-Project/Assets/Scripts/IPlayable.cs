using UnityEngine;

public interface IPlayable
{
    GameObject Character { get; }
    void Move(float sensitivity);
    void StopMoving();
}
