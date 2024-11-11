using JetBrains.Annotations;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackingState : StateBase
{
    public bool rotateLeft;

    public float rotate;

    public AttackingState(Player player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
        Turn();
    }

    void Turn()
    {
        rotateLeft = Player.TransformTail.rotation.eulerAngles.y >= 180? !rotateLeft : rotateLeft;
        Player.TransformTail.transform.RotateAround(Player.rb.position, Vector3.up, rotateLeft ? Player.TurnSpeed : -Player.TurnSpeed);
        rotate += Player.TurnSpeed;
        Debug.Log(rotate);        
    }

    public override void Enter()
    {
        rotate = 0;
    }

    public override void Exit()
    {
        Player.TransformTail.transform.RotateAround(Player.rb.position, Vector3.up, 0);
    }
}