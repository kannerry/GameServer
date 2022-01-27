using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class CardmasterStack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        private IObjAiBase own;
        ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, HideE, false);
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            own = owner;
            _spell = spell;
        }

        public void HideE(ISpell spell)
        {
            CreateTimer((float)0.1, () => { SealSpellSlot(own, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true); });
        }
        int i = 0;
        IParticle atkParticle;
        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            i++;
            if (i == 3)
            {
                if (own.Spells[2].CastInfo.SpellLevel != 0)
                {
                    atkParticle = AddParticle(own, own, "Cardmaster_stackready.troy", own.Position, lifetime: float.MaxValue);
                }
            }
            if (i == 4)
            {
                var dmg = 55 + 25 * _spell.CastInfo.SpellLevel;
                var ap = own.Stats.AbilityPower.Total * 0.5f;
                if(own.Spells[2].CastInfo.SpellLevel != 0)
                {
                    unit.TakeDamage(own, dmg + ap, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddParticle(unit, unit, "CardmasterStackAttack_tar.troy", unit.Position);
                    atkParticle.SetToRemove();
                }
                i = 0;
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
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