using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class BantamTrap : ISpellScript
    {
        public List<ISpellSector> mushroomRanges = new List<ISpellSector>();

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
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

            Passives.Camouflage.timer = 0;

            var owner = spell.CastInfo.Owner;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var mushroom = AddMinion(owner, "TeemoMushroom", "TeemoMushroom", spellPos);
            AddBuff("BantamTrap", 600f, 1, spell, mushroom, mushroom);

            mushroomRanges.Add(spell.CreateSpellSector(new SectorParameters
            {
                BindObject = mushroom,
                Length = 80f,
                Tickrate = 60,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 600f
            }));
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (mushroomRanges.Contains(sector))
            {
                var owner = spell.CastInfo.Owner;
                var mushroomObj = sector.Parameters.BindObject;

                AddBuff("NoxiousTrap", 4f, 1, spell, target, spell.CastInfo.Owner);
                if (mushroomObj is IAttackableUnit mushroom)
                {
                    AddParticle(owner, null, "ShroomMine.troy", mushroom.Position, 1.0f);
                    mushroom.TakeDamage(mushroom, 10000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                }
                sector.SetToRemove();
            }
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