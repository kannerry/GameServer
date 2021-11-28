using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;

namespace Buffs
{
    internal class ZhonyasHourglassBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle p;
        IParticle p2;
        float RealBaseArmor;
        float RealBaseMagicResist;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "zhonyas_ring_activate.troy", unit, buff.Duration, size: 2);
            p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "zhonya_ring_self_skin.troy", unit, buff.Duration, size: 2);
            var RealBaseArmor = ownerSpell.CastInfo.Owner.Stats.Armor.BaseValue;
            var RealBaseMagicResist = ownerSpell.CastInfo.Owner.Stats.MagicResist.BaseValue;
            ownerSpell.CastInfo.Owner.SetStatus(StatusFlags.Targetable, false);
            ownerSpell.CastInfo.Owner.SetStatus(StatusFlags.CanMove, false);
            ownerSpell.CastInfo.Owner.Stats.Armor.BaseValue = 20000;
            ownerSpell.CastInfo.Owner.Stats.MagicResist.BaseValue = 20000;
            ownerSpell.CastInfo.Owner.StopMovement();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
            ownerSpell.CastInfo.Owner.SetStatus(StatusFlags.Targetable, true);
            ownerSpell.CastInfo.Owner.SetStatus(StatusFlags.CanMove, true);
            ownerSpell.CastInfo.Owner.Stats.Armor.BaseValue = RealBaseArmor;
            ownerSpell.CastInfo.Owner.Stats.MagicResist.BaseValue = RealBaseMagicResist;
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
