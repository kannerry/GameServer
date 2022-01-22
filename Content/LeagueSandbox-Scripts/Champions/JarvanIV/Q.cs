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
    public class JarvanIVDragonStrike : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            ApiEventManager.OnFinishDash.AddListener(this, owner, disableKnockup, false);
        }
        bool Knockup;

        public void disableKnockup(IAttackableUnit unit)
        {
            Knockup = false;
            LogDebug("knockup bool set ot false");
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var damage = 50f + (40f * spell.CastInfo.SpellLevel - 1) + (owner.Stats.AttackDamage.Total * 1.25f);
            if (target.Model == "JarvanIVStandard")
            {
                LogDebug("hit standard");
                owner.SetTargetUnit(null);
                ForceMovement(owner, "RUN", target.Position, 2000, 0, 0, 0);
                Knockup = true;
            }
            if (target.Team != owner.Team)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                CreateTimer(0.1f, () =>
                {
                    if (Knockup == true)
                    {
                        AddBuff("Pulverize", 1.0f, 1, spell, target, owner);
                    }
                });
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        static internal bool isQing;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            isQing = true;
            CreateTimer(0.75f, () => { isQing = false; });
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            spell.CreateSpellSector(new SectorParameters
            {
                BindObject = owner,
                Length = 785f,
                Width = 150,
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

            spell.CreateSpellSector(new SectorParameters
            {
                BindObject = owner,
                Length = 861f,
                Width = 65,
                PolygonVertices = new Vector2[]
    {
                                // Basic square, however the values will be scaled by Length/Width, which will make it a rectangle
                                new Vector2(-1, 0),
                                new Vector2(-1, 1),
                                new Vector2(1, 1),
                                new Vector2(1, 0)
    },
                SingleTick = true,
                Type = SectorType.Polygon,
                OverrideFlags = SpellDataFlags.AffectFriends
            });

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