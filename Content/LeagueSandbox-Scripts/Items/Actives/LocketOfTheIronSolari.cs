using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemSpells
{
    public class IronStylus : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
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
            AddParticle(owner, owner, "Global_Item_RighteousGlory_Aoe.troy", owner.Position, lifetime: 5.0f);
            AddParticle(owner, owner, "AscForceBubble.troy", owner.Position, lifetime: 5.0f);
            var shieldamt = 50 + 10 * owner.Stats.Level;
            var c = GetChampionsInRange(owner.Position, 850, true);
            foreach(var champion in c)
            {
                if(champion.Team == owner.Team)
                {
                    AddParticle(champion, champion, "TEMP_SupportShield.troy", champion.Position, lifetime: 5.0f);
                    champion.ApplyShield(champion, shieldamt, true, true, false);
                    CreateTimer(5.0f, () => { champion.ApplyShield(champion, -shieldamt, true, true, false); });
                }
            }
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