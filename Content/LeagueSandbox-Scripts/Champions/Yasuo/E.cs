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
    public class YasuoDashWrapper : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public static IAttackableUnit _target = null;

        IObjAiBase _owner;

        public void OpenE(IAttackableUnit unit)
        {
            SealSpellSlot(_owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            ApiEventManager.OnFinishDash.AddListener(this, owner, OpenE, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
            //here's empty
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            _target = target;
            if (!target.HasBuff("YasuoEBlockFIX"))
            {
                SealSpellSlot(_owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
                AddBuff("YasuoEFIX", 0.395f - spell.CastInfo.SpellLevel * 0.012f, 1, spell, owner, owner);
                AddBuff("YasuoEBlockFIX", 11f - spell.CastInfo.SpellLevel * 1f, 1, spell, target, owner);
            }
        }

        public void OnSpellCast(ISpell spell)
        {
            //here's empty, maybe will add some functions?
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var x = (new float[] { 0.5f, 0.4f, 0.3f, 0.2f, 0.1f }[spell.CastInfo.SpellLevel - 1]);
            spell.SetCooldown(x, true);
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
            //here's empty because it's not working
        }
    }
}