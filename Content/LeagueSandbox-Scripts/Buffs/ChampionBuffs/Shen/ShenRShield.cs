using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class shenstandunitedshield : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            IObjAiBase unitAi = unit as IObjAiBase;
            IObjAiBase owner = ownerSpell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total * 1.35f;
            float spellvl = (ownerSpell.CastInfo.SpellLevel * 300) - 50;

            LogDebug(((ap + spellvl).ToString()));

            unitAi.ApplyShield(unit, (float)(ap + spellvl), true, true, false);
            AddParticle(unit, unit, "Shen_StandUnited_shield_v2.troy", unit.Position, lifetime: 3.0f, bone: "chest");
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            IObjAiBase unitAi = unit as IObjAiBase;
            IObjAiBase owner = ownerSpell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total * 1.35f;
            float spellvl = (ownerSpell.CastInfo.SpellLevel * 300) + 250;
            unitAi.ApplyShield(unit, -(float)(ap + spellvl), true, true, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}