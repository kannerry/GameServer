using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3071 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            StatsModifier.ArmorPenetration.FlatBonus += 10;
            StatsModifier.CooldownReduction.PercentBonus += 0.1f;
            owner.AddStatModifier(StatsModifier);
        }

        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            //AddBuff("BlackCleaverDebuff", 5.0f, 1, _owner.GetSpell(0), Unit, _owner);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}