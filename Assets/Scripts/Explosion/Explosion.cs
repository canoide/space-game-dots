using Unity.Entities;

[GenerateAuthoringComponent]
public struct Explosion : IComponentData
{
    public Entity Effect;
}
