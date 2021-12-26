using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class InsanityPotion : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle highlander;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var var1 = (new float[] { 35f, 50f, 80f }[ownerSpell.CastInfo.SpellLevel - 1]);

            highlander = AddParticleTarget(owner, unit, "Highlander_buf.troy", unit);

            StatsModifier.AbilityPower.FlatBonus += var1;
            StatsModifier.Armor.FlatBonus += var1;
            StatsModifier.MagicResist.FlatBonus += var1;
            StatsModifier.MoveSpeed.FlatBonus += var1;
            StatsModifier.HealthRegeneration.FlatBonus += var1;
            StatsModifier.ManaRegeneration.FlatBonus += var1;
            unit.AddStatModifier(StatsModifier);
            // TODO: add immunity to slows
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(highlander);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}