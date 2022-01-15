using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using Spells;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ShadowTimer : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            IObjAiBase obj = unit as IObjAiBase;
            TeleportTo(obj, 0, 0);

            unit.TakeDamage(unit, 10000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_RAW, false);

            if (ownerSpell.SpellName == "ZedUlt")
            {
                if (Spells.ZedR2.recast == false)
                {
                    owner.SetSpell("ZedUlt", 3, true);
                    var cd = (new float[] { 120f, 100f, 80f }[owner.GetSpell(3).CastInfo.SpellLevel - 1]);
                    owner.GetSpell(3).SetCooldown(cd);
                    ZedShadowDashMissile.RShadow = null;
                }
                else
                {
                    Spells.ZedR2.recast = false;
                    ZedShadowDashMissile.RShadow = null;
                }
            }

            if (ownerSpell.SpellName == "ZedShadowDash")
            {
                if(Spells.ZedW2.recast == false)
                {
                    owner.SetSpell("ZedShadowDash", 1, true);
                    var cd = (new float[] { 18f, 17f, 16f, 15f, 14f }[owner.GetSpell(1).CastInfo.SpellLevel - 1]);
                    owner.GetSpell(1).SetCooldown(cd);
                    ZedShadowDashMissile.WShadow = null;
                }
                else
                {
                    Spells.ZedW2.recast = false;
                    ZedShadowDashMissile.WShadow = null;
                }
            }

        }

        public void OnUpdate(float diff)
        {
        }
    }
}