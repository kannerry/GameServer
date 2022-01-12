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
    public class SionR : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        IChampion _owner;
        ISpell _spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            _owner = owner as IChampion;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            ulting = true;
            CreateTimer(8.0f, () => { spawnFix = true; ulting = false; ForceMovement(_owner, "", GetPointFromUnit(owner, 10), 950, 0, 0, 0); });
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

        bool ulting = false;
        bool spawnFix = false;
        public void OnUpdate(float diff)
        {
            if (ulting == true)
            {
                FaceDirection(_owner.SpellChargeXY, _owner);
                var movepos = GetPointFromUnit(_owner, 50);
                ForceMovement(_owner, "", movepos, 950, 0, 0, 0, ForceMovementType.FIRST_WALL_HIT);
                foreach(var unit in GetUnitsInRange(_owner.Position, 100, true))
                {
                    if(unit.Team != _owner.Team)
                    {
                        ulting = false;
                        spawnFix = true;
                        AddBuff("Pulverize", 1.0f, 1, _spell, unit, _owner);
                        _owner.SetTargetUnit(unit);
                    }

                }
            }
            if(spawnFix == true)
            {
                var fixmin = AddMinion(_owner, "TestCube", "fix", _owner.Position);
                _owner.SetTargetUnit(fixmin);
                CreateTimer(0.5f, () => { fixmin.TakeDamage(_owner, 10000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_RAW, true); });
                spawnFix = false;
            }
        }
    }
}