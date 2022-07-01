using SpaceArcade.Movement;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceArcade.PlayerInput
{
    public partial class PlayerMovementInputSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(ComponentType.ReadOnly<PlayerTag>(), 
                typeof(MovementDirection)));
        }

        protected override void OnUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            Entities
                .WithBurst()
                .ForEach((ref MovementDirection playerMovement, in PlayerTag playerTag) =>
                    playerMovement.Value = new float3(horizontal, 0, vertical))
                .Schedule();
        }
    }
}