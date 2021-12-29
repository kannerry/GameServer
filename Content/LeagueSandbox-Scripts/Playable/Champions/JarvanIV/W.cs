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
    public class JarvanIVGoldenAegis : ISpellScript
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

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            int i = 0;
            var x = GetChampionsInRange(owner.Position, 600, true);
            foreach(var unit in x)
            {
                if(unit.Team != owner.Team)
                {
                    AddBuff("JarvanW", 2.0f, 1, spell, unit, owner);
                    i++;
                }
            }

            AddParticle(owner, owner, "JarvanGoldenAegis_nova.troy", owner.Position);

            var dmgshield = 10 + (40 * spell.CastInfo.SpellLevel) + (i * (20 + (10 * spell.CastInfo.SpellLevel)));
            //LogDebug(dmgshield.ToString());
            owner.ApplyShield(owner, dmgshield, true, true, false);
            CreateTimer(5.0f, () => { owner.ApplyShield(owner, -dmgshield, true, true, false); });
            i = 0;

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