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
    public class KhazixW : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var Champs = GetAllChampionsInRange(owner.Position, 50000);
            foreach (IChampion player in Champs)
            {
                owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0f);
                owner.SetStatus(StatusFlags.Targetable, true);
                owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true);
            }
            FaceDirection(end, owner);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var endPos = GetPointFromUnit(spell.CastInfo.Owner, 925);
            SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, endPos, endPos, false, Vector2.Zero);
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

    public class KhazixWMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            }
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
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

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total * spell.SpellData.MagicDamageCoefficient;
            float healthGain = 15 + (spell.CastInfo.SpellLevel * 45) + ap;
            if (target.HasBuff("HealCheck"))
            {
                healthGain *= 0.5f;
            }
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ad = owner.Stats.AttackDamage.Total + spell.CastInfo.SpellLevel - 1 * 20;
            var x = GetUnitsInRange(target.Position, 275, true);
            AddParticle(owner, null, "Khazix_Base_W_Tar.troy", target.Position);
            foreach (var unit in x)
            {
                if(unit == owner)
                {
                    LogDebug("HEAL");
                    AddParticle(owner, null, "Khazix_Base_W_Heal.troy", owner.Position);
                    PerformHeal(owner, spell, owner);
                }
                if(unit.Team != owner.Team)
                {
                    unit.TakeDamage(owner, ad, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddBuff("KhazixWSlow", 2.0f, 1, spell, unit, owner);
                }
            }
            missile.SetToRemove();
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