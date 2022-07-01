using SpaceArcade.Weapon;
using Unity.Entities;
using UnityEngine;

namespace SpaceArcade.PlayerInput
{
    public partial class PlayerFireInputSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(ComponentType.ReadOnly<PlayerTag>(), 
                typeof(Gun)));
        }

        protected override void OnUpdate()
        {
            var fire = Input.GetButton("Fire1");
            Entities
                .WithBurst()
                .ForEach((ref Gun gun, in PlayerTag playerTag) =>
                    gun.Fire = fire)
                .Schedule();
        }
    }
}