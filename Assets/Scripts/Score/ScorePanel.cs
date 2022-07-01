using TMPro;
using Unity.Entities;

namespace SpaceArcade.Score
{
    [GenerateAuthoringComponent]
    public class ScorePanel : IComponentData
    {
        public TextMeshProUGUI Text;
        public int SetScoreValue;
    }
}