using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class TwitchExpunge : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        private IObjAiBase own;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, HideE, false);
            own = owner;
        }

        public void HideE(ISpell spell)
        {
            CreateTimer(0.1f, () => { SealSpellSlot(own, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true); });
        }

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            //var ap = own.Stats.AbilityPower.Total;
            //float damage = (float)(ap * 0.3 + own.GetSpell(2).CastInfo.SpellLevel * 10);
            //unit.TakeDamage(own, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var damage = 10 + 20 * spell.CastInfo.SpellLevel;
            var units = GetUnitsInRange(owner.Position, 1000, true);
            foreach (var unit in units)
            {
                if (unit.HasBuff("TwitchPassive"))
                {
                    var stackdmg = unit.GetBuffWithName("TwitchPassive").StackCount * 15 + 10;
                    SpellCast(spell.CastInfo.Owner, 2, SpellSlotType.ExtraSlots, spell.CastInfo.Owner.Position, unit.Position, true, Vector2.Zero);
                    unit.TakeDamage(owner, damage + stackdmg, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                }
            }
        }

        public void OnSpellCast(ISpell spell)
        {
            //SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }

    public class TwitchEParticle : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
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

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
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