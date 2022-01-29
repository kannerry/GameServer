using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ShenFeint : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IAttackableUnit _shen;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //KiCooldown
            //_shen = unit;
            //ApiEventManager.OnHitUnit.AddListener(this, unit as IObjAiBase, LowerPassive, false);
            IObjAiBase unitAi = unit as IObjAiBase;
            float ap = unit.Stats.AbilityPower.Total * 0.6f;
            float spellvl = (ownerSpell.CastInfo.SpellLevel * 40) + 20;
            unitAi.ApplyShield(unitAi, (float)(ap + spellvl), true, true, false);
            AddParticle(unit, unit, "shen_Feint_self.troy", unit.Position, lifetime: 3.0f);
            LogDebug((ap + spellvl).ToString());
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);
            IObjAiBase unitAi = unit as IObjAiBase;
            float ap = unit.Stats.AbilityPower.Total * 0.6f;
            float spellvl = (ownerSpell.CastInfo.SpellLevel * 40) + 20;
            unitAi.ApplyShield(unitAi, -(float)(ap + spellvl), true, true, false);
        }

        //public void LowerPassive(IAttackableUnit unit, bool crit)
        //{
        //    var buff = _shen.GetBuffWithName("KiCooldown");
        //    if(buff != null)
        //    {
        //        var startTime = buff.TimeElapsed;
        //    }
        //}

        public void OnUpdate(float diff)
        {
        }
    }
}