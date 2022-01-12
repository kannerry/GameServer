using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class RaiseMorale : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
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
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var hasbuff = spell.CastInfo.Owner.HasBuff("RaiseMorale");

            if (hasbuff == false)
            {
                AddBuff("RaiseMorale", 7.0f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
            }

            var units = GetUnitsInRange(spell.CastInfo.Owner.Position, 1000, true).Where(x => x.Team != CustomConvert.GetEnemyTeam(spell.CastInfo.Owner.Team));
            foreach (var allyTarget in units)
            {
                if (allyTarget is IAttackableUnit && spell.CastInfo.Owner != allyTarget && hasbuff == false)
                {
                    AddBuff("RaiseMorale", 7.0f, 1, spell, allyTarget, spell.CastInfo.Owner);
                }
            }
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