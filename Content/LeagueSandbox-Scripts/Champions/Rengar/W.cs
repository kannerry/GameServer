using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class RengarW : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            float baseHP = owner.Stats.HealthPoints.Total + 1;
            float missingHP = owner.Stats.CurrentHealth + 1;
            float PercentMissingHP = ((baseHP - missingHP) / baseHP * 100);
            //LogDebug(PercentMissingHP.ToString());
            int i = 0;
            while (i < PercentMissingHP)
            {
                i++;
            }
            float x = i * 6.25f;

            float healMin = 8 + 4 * owner.Stats.Level;
            float healMax = 50 + 25 * owner.Stats.Level;
            float healAdditive = healMin * (x* 0.01f);

            float healAmount = Math.Min(healAdditive, healMax);

            target.Stats.CurrentHealth = Math.Min(healAmount + target.Stats.CurrentHealth, target.Stats.HealthPoints.Total);
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {

            var ownerObj = owner as IAttackableUnit;
            PlayAnimation(owner, "Spell2");
            AddParticle(owner, owner, "Rengar_Base_W_Roar.troy", owner.Position);
            var x = GetUnitsInRange(owner.Position, 450, true);

            AddBuff("RengarWBuff", 4.0f, 1, spell, owner, owner);

            if (spell.CastInfo.Owner.Stats.CurrentMana == 5)
            {
                LogDebug("heal");
                PerformHeal(owner, spell, owner);
                spell.CastInfo.Owner.Stats.CurrentMana = 1;
            }
            else
            {
                spell.CastInfo.Owner.Stats.CurrentMana += 1;
            }

            var basedmg = 20 + 30 * spell.CastInfo.SpellLevel;
            var apscaling = owner.Stats.AbilityPower.Total * 0.8f;
            foreach(var unit in x)
            {
                if(unit.Team != owner.Team)
                {
                    unit.TakeDamage(owner, basedmg + apscaling, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            spell.SetCooldown(12f, true);
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}