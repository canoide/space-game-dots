using Unity.Entities;
using Unity.Transforms;

namespace SpaceArcade.Weapon
{
    public partial class GunSystem : SystemBase
    {
        EntityCommandBufferSystem m_EntityCommandBufferSystem;
        
        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
            RequireForUpdate(GetEntityQuery(typeof(Gun), typeof(LocalToWorld)));
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            
            var dt = Time.DeltaTime;
            Entities
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, ref Gun weapon, in LocalToWorld gunTransform) =>
                {
                    weapon.Duration -= dt;
                    if (!weapon.Fire) return;
                    if (weapon.Duration > 0) return;
                    weapon.Duration = weapon.Rate;
                    var projectileEntity = commandBuffer.Instantiate(entityInQueryIndex, weapon.Projectile);
                    var position = new Translation { Value = gunTransform.Position };
                    commandBuffer.SetComponent(entityInQueryIndex, projectileEntity, position);
                })
                .ScheduleParallel();
            
            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}