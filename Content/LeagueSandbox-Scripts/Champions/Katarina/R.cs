using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class KatarinaR : ISpellScript
    {

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2.5f,
        };

        private Vector2 basepos;

        public void OnActivate(IObjAiBase owner, ISpell spell)
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
            var Owner = spell.CastInfo.Owner;
            AddBuff("KatarinaR", 2.5f, 1, spell, Owner, Owner);
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
            var Owner = spell.CastInfo.Owner;
            RemoveBuff(Owner, "KatarinaR");
            LogDebug("OnSpellChannelCancel");
        }

        public void OnSpellPostChannel(ISpell spell)
        {
            LogDebug("OnSpellPostChannel");
            var owner = spell.CastInfo.Owner;
            owner.StopAnimation("Spell4", fade: true);

            RemoveBuff(owner, "KatarinaR");

        }

        public void OnUpdate(float diff)
        {
        }
    }
}