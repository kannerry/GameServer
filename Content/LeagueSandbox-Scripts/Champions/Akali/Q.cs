using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class AkaliMota : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            TriggersSpellCasts = true

            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            AddBuff("PassiveCooldownAkali", 8.0f, 1, spell, owner, owner);
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, deezAttack, false);
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        bool attack = true;
        public void deezAttack(ISpell spell)
        {
            var target = spell.CastInfo.Targets[0].Unit;
            var owner = spell.CastInfo.Owner;
            var AP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.3f;
            var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 40 + spell.CastInfo.SpellLevel * 30 + AP + AD;
            var MarkAPratio = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.5f;
            var MarkDamage = 45 + 25 * (owner.GetSpell("AkaliMota").CastInfo.SpellLevel - 1) + MarkAPratio;

            if (target.HasBuff("AkaliMota"))
            {
                owner.Stats.CurrentMana += 40;
                target.TakeDamage(owner, MarkDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
                AddParticleTarget(owner, target, "akali_mark_impact_tar.troy", target, 1f);
                RemoveBuff(target, "AkaliMota");
            }

            //PASSIVE

            if (owner.HasBuff("AkaliTwinDisciplines"))
            {
                attack = !attack;
                LogDebug(attack.ToString());
                if (attack == true)
                {
                    var ap = owner.Stats.AbilityPower.Total * 0.5f;
                    var ad = owner.Stats.AttackDamage.Total * 0.75f;
                    var swagDMG = 8 + (spell.CastInfo.Owner.Stats.Level * 2) + ap + ad;

                    target.TakeDamage(owner, swagDMG, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
                    owner.RemoveBuffsWithName("AkaliTwinDisciplines");
                    AddBuff("PassiveCooldownAkali", 8.0f, 1, spell, owner, owner);
                }
                if (attack == false)
                {
                    PerformHeal(owner, spell, owner);
                }
            }

        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total * 0.45f;
            var ad = owner.Stats.AttackDamage.Total * 0.6f;
            float healthGain = 3 + (spell.CastInfo.Owner.Stats.Level * 2) + ap + ad;
            if (target.HasBuff("HealCheck"))
            {
                healthGain *= 0.5f;
            }
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.4f;
            var damage = 15f + spell.CastInfo.SpellLevel * 20f + AP;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddBuff("AkaliMota", 6f, 1, spell, target, owner);
            missile.SetToRemove();

            if (AkaliSmokeBomb.SwagSector != null)
            {
                AkaliSmokeBomb.SwagSector.SetToRemove();
                AkaliSmokeBomb.smokeBomb.SetToRemove();
                AkaliSmokeBomb.smokeBombBorder.SetToRemove();
            }

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