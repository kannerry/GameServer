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
    public class VelkozR : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        IChampion _owner;
        ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {

            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);

            _owner = owner as IChampion;
            _spell = spell;
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            float apscaling = owner.Stats.AbilityPower.Total * 0.6f;
            float damage = 300 + spell.CastInfo.SpellLevel * 200;
            target.TakeDamage(owner, (damage + apscaling) / 50, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL,
                false);
            i++;
            LogDebug(i.ToString());
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        bool ulting = false;
        int i = 0;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            ulting = true;
            CreateTimer(3.0f, () => { ForceMovement(owner, "run", new Vector2(owner.Position.X + 10, owner.Position.Y + 10), 1000, 0, 0, 0); ulting = false; });
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
            if(ulting == true)
            {
                FaceDirection(_owner.SpellChargeXY, _owner);
                AddParticle(_owner, _owner, "Velkoz_Base_R_beam.troy", GetPointFromUnit(_owner, 1555), lifetime: 0.025f, reqVision: false);
                _spell.CreateSpellSector(new SectorParameters
                {
                    BindObject = _owner,
                    Length = 1555f,
                    Width = 175f,
                    PolygonVertices = new Vector2[]
                    {
                        // Basic square, however the values will be scaled by Length/Width, which will make it a rectangle
                        new Vector2(-1, 0),
                        new Vector2(-1, 1),
                        new Vector2(1, 1),
                        new Vector2(1, 0)
                    },
                    SingleTick = true,
                    Type = SectorType.Polygon
                });
            }
        }
    }
}