using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class AhriFoxFire : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
        }

        IObjAiBase _owner;

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void RemoveStacks(int var)
        {
            int x = 0;
            foreach (var swag in _owner.GetBuffsWithName("AhriSoulCrusherCounter"))
            {
                if (x < var)
                {
                    x++;
                    swag.DeactivateBuff();
                }
            }
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

        public void OnSpellPostCast(ISpell spell)
        {

            if (spell.CastInfo.Owner.GetBuffWithName("AhriSoulCrusherCounter").StackCount == 9)
            {
                PerformHeal(spell.CastInfo.Owner, spell, spell.CastInfo.Owner);
                CreateTimer(0.1f, () => { RemoveStacks(9); });
            }

            var units = GetUnitsInRange(spell.CastInfo.Owner.Position, 1000, true).Where(x => x.Team == CustomConvert.GetEnemyTeam(spell.CastInfo.Owner.Team));
            var i = 0;
            foreach (var allyTarget in units)
            {
                if (allyTarget is IAttackableUnit && spell.CastInfo.Owner != allyTarget)
                {
                    if (i < 3)
                    {
                        SpellCast(spell.CastInfo.Owner, 3, SpellSlotType.ExtraSlots, true, allyTarget, Vector2.Zero);
                        AddBuff("AhriSoulCrusherCounter", float.MaxValue, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
                        LogDebug(spell.CastInfo.Owner.GetBuffWithName("AhriSoulCrusherCounter").StackCount.ToString());
                        i++;
                    }
                }
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

    public class AhriFoxFireMissileTwo : ISpellScript
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

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ad = owner.Stats.AbilityPower.Total * 0.4;
            var damage = ad + 30 * owner.Spells[0].CastInfo.SpellLevel;
            target.TakeDamage(owner, (float)damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
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