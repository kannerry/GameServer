using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class YasuoQ02 : IBuffGameScript
    {
        public BuffType BuffType => BuffType.INTERNAL;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => true;

        public IStatsModifier StatsModifier { get; private set; }

        private IParticle p1;
        private IParticle p2;
        private IParticle p3;
        private IParticle p4;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;
            ((IChampion)unit).SetSpell("YasuoQ3W", 0, true);
            ((IObjAiBase)unit).GetSpell(0).SetCooldown(1.33f);
            p1 = AddParticleTarget(caster, (IChampion)unit, "Yasuo_Base_Q3_Indicator_Ring.troy", unit, lifetime: 10.0f, teamOnly: unit.Team);
            p2 = AddParticleTarget(caster, (IChampion)unit, "Yasuo_Base_Q3_Indicator_Ring_alt.troy", unit, lifetime: 10.0f, teamOnly: unit.Team);
            p3 = AddParticleTarget(caster, (IChampion)unit, "Yasuo_Base_Q_wind_ready_buff.troy", unit, lifetime: 10.0f);
            p4 = AddParticleTarget(caster, (IChampion)unit, "Yasuo_Base_Q_strike_build_up_test.troy", unit, lifetime: 10.0f);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (((IChampion)unit).Spells[0].SpellName == "YasuoQ3W")
            {
                ((IChampion)unit).SetSpell("YasuoQW", 0, true);
            }
            RemoveParticle(p1);
            RemoveParticle(p2);
            RemoveParticle(p3);
            RemoveParticle(p4);
        }

        public void OnUpdate(float diff)
        {
            //empty!
        }
    }
}