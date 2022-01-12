using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class TwitchDeadlyVenomMarker : ICharScript
    {
        ISpell _spell;
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            _owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHit, false);
        }

        public void OnHit(IAttackableUnit unitHit, bool crit)
        {
            AddBuff("TwitchPassive", 6.0f, 1, _spell, unitHit, _spell.CastInfo.Owner);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            var units = GetUnitsInRange(_owner.Position, 1000, true);
            foreach(var unit in units)
            {
                if (unit.HasBuff("TwitchPassive"))
                {
                    //LogDebug(unit.Model.ToString());
                    SealSpellSlot(_owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
                    CreateTimer(.01f, () => 
                    {
                        if (!unit.HasBuff("TwitchPassive"))
                        {
                            SealSpellSlot(_owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
                        }
                    });
                }
            }
        }
    }
}