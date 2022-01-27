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
    public class FlashFrost : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            // TODO
        };

        static internal ISpell _spellref;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }


        // THIS IS THE WORK OF THE DEVIL
        // SORRY ABOUT THAT

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            //ProcDMG
            LogDebug("yoProcDMG");
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            FaceDirection(end, owner);
            LogDebug("YO");
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, GetPointFromUnit(owner, 1100), GetPointFromUnit(owner, 1100), false, Vector2.Zero);

            CreateTimer(0.5f, () => { owner.SetSpell("Crystallize", 0, true); });

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

    public class FlashFrostSpell : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            FlashFrost._spellref = spell;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            ApiEventManager.OnSpellMissileEnd.AddListener(this, GetObjectNET(spell.CastInfo.MissileNetID) as ISpellMissile, EndFix, false);
        }

        public void EndFix(ISpellMissile missile)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var dmg = owner.Stats.AbilityPower.Total * 0.5f;
            target.TakeDamage(owner, dmg + 60, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            CreateTimer(1.25f, () => 
            { 
                LogDebug("yo2"); 
                owner.SetSpell("FlashFrost", 0, true);
                owner.GetSpell(0).SetCooldown(15f);
                if (Crystallize.recasted == false)
                {
                    AddParticlePos(owner, "cryo_FlashFrost_tar.troy", end, end);
                    var unitss = GetUnitsInRange(end, 200, true);
                    foreach (var unit in unitss)
                    {
                        if (unit.Team != owner.Team)
                        {
                            var dmg = owner.Stats.AbilityPower.Total * 0.5f;
                            unit.TakeDamage(owner, dmg + 60, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                            AddBuff("Stun", 1.0f, 1, spell, unit, owner);
                        }
                    }
                }
                else
                {
                    Crystallize.recasted = false;
                }
            });
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