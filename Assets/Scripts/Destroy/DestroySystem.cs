using Unity.Entities;

namespace SpaceArcade.Destroy
{
    public partial class DestroySystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;
        
        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            RequireForUpdate(GetEntityQuery(typeof(DestroyTag)));
        }
        
        protected override void OnUpdate()
        {
            var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            
            Entities
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, ref DestroyTag destroyByTimeComponent) =>
                {
                    commandBuffer.DestroyEntity(entityInQueryIndex, entity);
                })
                .Schedule();
            
            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}