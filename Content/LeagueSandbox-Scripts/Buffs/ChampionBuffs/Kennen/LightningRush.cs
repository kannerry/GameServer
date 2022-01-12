using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class KennenLightningRush : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IAttackableUnit owner;
        private ISpell originSpell;
        private IBuff thisBuff;
        private IParticle red;
        private IParticle green;
        public ISpellSector AuraKennen1;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = unit;
            originSpell = ownerSpell;
            thisBuff = buff;

            red = AddParticleTarget(owner, unit, "kennen_lr_buf.troy", unit, buff.Duration); //Take a look at whi the particles disapear later
            green = AddParticleTarget(owner, unit, "Kennen_lr_tar.troy", unit, buff.Duration);

            StatsModifier.MoveSpeed.PercentBonus += 1.0f;
            unit.AddStatModifier(StatsModifier);
            unit.SetStatus(StatusFlags.Ghosted, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(red);
            RemoveParticle(green);
            unit.SetStatus(StatusFlags.Ghosted, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}