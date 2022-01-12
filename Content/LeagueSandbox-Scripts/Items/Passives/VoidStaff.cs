using GameServerCore.Domain.GameObjects;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace ItemPassives
{
    public class ItemID_3135 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner;
            StatsModifier.MagicPenetration.PercentBonus = 0.35f;
            owner.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}