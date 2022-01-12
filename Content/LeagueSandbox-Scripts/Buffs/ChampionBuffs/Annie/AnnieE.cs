using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class AnnieEBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle _particle;
        private ISpell _ownerSpell;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _ownerSpell = ownerSpell;
            _particle = AddParticleTarget(unit, unit, "Annie_E_buf.troy", unit, buff.Duration);

            float bonus = 10f + (10f * ownerSpell.CastInfo.Owner.Stats.Level);
            StatsModifier.Armor.FlatBonus += bonus;
            StatsModifier.MagicResist.FlatBonus += bonus;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(_particle);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}