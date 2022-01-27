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
    public class RivenTriCleave : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnFinishDash.AddListener(this, owner, DashFin, false);
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var Adratio = owner.Stats.AttackDamage.FlatBonus * 0.9f;
            var damage = 40 * (spell.CastInfo.SpellLevel) + Adratio;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            if(dash == 3)
            {
                AddBuff("Pulverize", 0.75f, 1, spell, target, owner);
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void DashFin(IAttackableUnit unit)
        {
            if(dash == 3)
            {
                DamageSector = _spell.CreateSpellSector(new SectorParameters
                {
                    BindObject = _owner,
                    Length = 200f,
                    Tickrate = 50,
                    CanHitSameTarget = false,
                    OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                    Type = SectorType.Area,
                    Lifetime = 0.5f
                });
            }
        }
        ISpellSector DamageSector;
        int q = 0;
        IObjAiBase _owner;
        ISpell _spell;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            _owner = owner;
            _spell = spell;
            var x = GetChampionsInRange(end, 200, true);
            foreach(var champ in x)
            {
                if(champ.Team != owner.Team)
                {
                    FaceDirection(champ.Position, owner);
                }
            }
        }

        public void OnSpellCast(ISpell spell)
        {
        }
        int dash = 0;
        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("RivenQRecast", 4.0f, 1, spell, owner, owner as IObjAiBase);
            var getbuff = owner.GetBuffWithName("RivenQRecast");
            switch (getbuff.StackCount)
            {
                case 1:
                    dash = 1;
                    PlayAnimation(owner, "Spell1A", 0.75f);
                    ForceMovement(owner, "Spell1A", GetPointFromUnit(owner, 225), 700, 0, 0, 0);
                    AddParticle(owner, owner, "Riven_Base_Q_01_Wpn_Trail.troy", owner.Position, bone: "chest");
                    DamageSector = spell.CreateSpellSector(new SectorParameters
                    {
                        BindObject = owner,
                        Length = 150f,
                        Tickrate = 50,
                        CanHitSameTarget = false,
                        OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                        Type = SectorType.Area,
                        Lifetime = 0.5f
                    });
                    return;
                case 2:
                    dash = 2;
                    PlayAnimation(owner, "Spell1B", 0.75f);
                    ForceMovement(owner, "Spell1B", GetPointFromUnit(owner, 225), 700, 0, 0, 0);
                    AddParticle(owner, owner, "Riven_Base_Q_02_Wpn_Trail.troy", owner.Position, bone: "chest");
                    DamageSector = spell.CreateSpellSector(new SectorParameters
                    {
                        BindObject = owner,
                        Length = 150f,
                        Tickrate = 50,
                        CanHitSameTarget = false,
                        OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                        Type = SectorType.Area,
                        Lifetime = 0.5f
                    });
                    return;
                case 3:
                    dash = 3;
                    PlayAnimation(owner, "Spell1C", 0.75f);
                    ForceMovement(owner, "Spell1C", GetPointFromUnit(owner, 250), 700, 0, 50, 0);
                    AddParticle(owner, owner, "Riven_Base_Q_03_Wpn_Trail.troy", owner.Position, size: -1);
                    return;
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