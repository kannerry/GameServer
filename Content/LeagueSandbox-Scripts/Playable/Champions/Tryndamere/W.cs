using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class MockingShout : ISpellScript
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

        private bool triggerFacing;

        public void OnSpellPostCast(ISpell spell)
        {
            triggerFacing = false;
            var owner = spell.CastInfo.Owner as IChampion;
            var units = GetUnitsInRange(owner.Position, 850f, true);
            foreach (var unit in units)
            {
                bool triggered = false;
                if (unit.Team != owner.Team)
                {
                    //FaceDirection(owner.Position, unit);
                    var point = GetPointFromUnit(unit, 825);
                    var champs = GetChampionsInRange(point, 825, true);
                    foreach (var champ in champs)
                    {
                        if (champ.NetId == owner.NetId)
                        {
                            triggered = true;
                        }
                    }
                    if (triggered != true)
                    {
                        LogDebug("yo");
                        AddBuff("MockingShoutSlow", 4f, 1, spell, unit, owner);
                        AddParticleTarget(owner, unit, "Chicken_buf.troy", unit, 4f);
                    }
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