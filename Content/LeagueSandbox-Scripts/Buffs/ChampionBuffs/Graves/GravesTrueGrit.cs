using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class GravesPassiveGrit : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_RENEWS;
        public int MaxStacks => 10;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle p;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            float stackamt = 1;
            if (unit.Stats.Level >= 7)
            {
                stackamt++;
            }
            if (unit.Stats.Level >= 13)
            {
                stackamt++;
            }
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "GravesTrueGrit.troy", unit, buff.Duration);
            StatsModifier.Armor.FlatBonus = stackamt;
            StatsModifier.MagicResist.FlatBonus = stackamt;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            float stackamt = 1;
            if (unit.Stats.Level >= 7)
            {
                stackamt++;
            }
            if (unit.Stats.Level >= 13)
            {
                stackamt++;
            }
            var toremove = stackamt * (buff.StackCount - 1);
            unit.Stats.Armor.FlatBonus -= toremove;
            unit.Stats.MagicResist.FlatBonus -= toremove;
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}