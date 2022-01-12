using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class SpectresCowl : IBuffGameScript
    {
        public BuffType BuffType => BuffType.HEAL;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle potion;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;
            StatsModifier.HealthRegeneration.FlatBonus = caster.Stats.HealthRegeneration.Total;
            unit.AddStatModifier(StatsModifier);
            potion = AddParticleTarget(caster, unit, "GLOBAL_Item_HealthPotion.troy", unit, buff.Duration, bone: "Buffbone_Glb_Ground_Loc");
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            potion.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
        }
    }
}