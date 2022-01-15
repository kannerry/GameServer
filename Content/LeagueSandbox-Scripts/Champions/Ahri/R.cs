using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class AhriTumble : ISpellScript
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
        int proc = 0;
        public void OnSpellPostCast(ISpell spell)
        {
            if(proc < 2)
            {
                proc++;
                CreateTimer(0.3f, () => { spell.SetCooldown(0); });
            }

            CreateTimer(10.0f, () => { proc = 0; });

            var current = new Vector2(spell.CastInfo.Owner.Position.X, spell.CastInfo.Owner.Position.Y);
            var trueCoords = GetPointFromUnit(spell.CastInfo.Owner, spell.SpellData.CastRangeDisplayOverride);

            CreateTimer(0.3f, () =>
            {
                var units = GetUnitsInRange(spell.CastInfo.Owner.Position, 500, true).Where(x => x.Team == CustomConvert.GetEnemyTeam(spell.CastInfo.Owner.Team));
                var i = 0;
                foreach (var allyTarget in units)
                {
                    if (allyTarget is IAttackableUnit && spell.CastInfo.Owner != allyTarget)
                    {
                        if (i < 1)
                        {
                            AddBuff("AhriSoulCrusherCounter", float.MaxValue, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
                            SpellCast(spell.CastInfo.Owner, 3, SpellSlotType.ExtraSlots, true, allyTarget, Vector2.Zero);
                            i++;
                        }
                    }
                }
            });

            FaceDirection(current, spell.CastInfo.Owner, true);
            spell.CastInfo.Owner.SetTargetUnit(null);
            ForceMovement(spell.CastInfo.Owner, "Spell4", trueCoords, 1500, 0, 0, 0);
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