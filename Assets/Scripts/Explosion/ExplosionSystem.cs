using SpaceArcade.Destroy;
using Unity.Entities;
using Unity.Transforms;

public partial class ExplosionSystem : SystemBase
{
    private BeginInitializationEntityCommandBufferSystem m_CommandBufferSystem;

    protected override void OnCreate()
    {
        m_CommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        RequireForUpdate(GetEntityQuery(typeof(DestroyTag), typeof(Explosion), typeof(Translation)));
    }

    protected override void OnUpdate()
    {
        var commandBuffer = m_CommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        Entities
            //.WithBurst()
            .WithoutBurst()
            .ForEach((Entity entity, int entityInQueryIndex, in DestroyTag destroyTag,
                in Translation translation, in Explosion explosion) =>
            {

                var instance = commandBuffer.Instantiate(entityInQueryIndex, explosion.Effect);
                var position = new Translation { Value = translation.Value };
                commandBuffer.SetComponent(entityInQueryIndex, instance, position);
            })
            .ScheduleParallel();
        m_CommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
