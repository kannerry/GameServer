using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Crystallize : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void ExecuteSpell(ISpell spell)
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

        // THIS IS THE WORK OF THE DEVIL
        // SORRY ABOUT THAT

        static internal bool recasted = false;
        public void OnSpellPostCast(ISpell spell)
        {
            if (spell.CastInfo.SpellSlot == 0)
            {
                var owner = spell.CastInfo.Owner;
                //FlashFrostSpell
                var mis = GetObjectNET(FlashFrost._spellref.CastInfo.MissileNetID);
                recasted = true;
                AddParticlePos(owner, "cryo_FlashFrost_tar.troy", mis.Position, mis.Position);
               var unitss = GetUnitsInRange(mis.Position, 200, true);
                foreach(var unit in unitss)
                {
                    if(unit.Team != owner.Team)
                    {
                        var dmg = owner.Stats.AbilityPower.Total * 0.5f;
                        unit.TakeDamage(owner, dmg + 60, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddBuff("Stun", 1.0f, 1, spell, unit, owner);
                    }
                }
                CreateTimer(0.1f, () => { GetObjectNET(FlashFrost._spellref.CastInfo.MissileNetID).SetToRemove(); });

                LogDebug("yoRecast");
                CreateTimer(0.5f, () => { LogDebug("yo2"); owner.SetSpell("FlashFrost", 0, true);});
                CreateTimer(0.51f, () => { LogDebug("yo3"); owner.GetSpell(0).SetCooldown(15f); });
            }
            if (spell.CastInfo.SpellSlot == 1)
            {
                SpawnWall(spell);
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

        public void SpawnWall(ISpell spell)
        {
            var spellLVL = spell.CastInfo.SpellLevel;
            var owner = spell.CastInfo.Owner;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var facingPos = GetPointFromUnit(spell.CastInfo.Owner, 950);

            IMinion m = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", spellPos, targetable: false);
            FaceDirection(facingPos, m);

            var LPos = GetPointFromUnit(m, spellLVL * 80, -90);
            var RPos = GetPointFromUnit(m, spellLVL * 80, 90);

            IMinion L = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", LPos, targetable: false);
            IMinion R = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", RPos, targetable: false);

            var L1Pos = GetPointFromUnit(m, spellLVL * 20, -90);
            var R1Pos = GetPointFromUnit(m, spellLVL * 20, 90);

            IMinion L1 = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", L1Pos, targetable: false);
            IMinion R1 = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", R1Pos, targetable: false);

            var L2Pos = GetPointFromUnit(m, spellLVL * 40, -90);
            var R2Pos = GetPointFromUnit(m, spellLVL * 40, 90);

            IMinion L2 = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", L2Pos, targetable: false);
            IMinion R2 = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", R2Pos, targetable: false);

            var L3Pos = GetPointFromUnit(m, spellLVL * 60, -90);
            var R3Pos = GetPointFromUnit(m, spellLVL * 60, 90);

            IMinion L3 = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", L3Pos, targetable: false);
            IMinion R3 = AddMinion((IChampion)owner, "AniviaIceBlock", "AniviaIceBlock", R3Pos, targetable: false);

            m.SetCollisionRadius(1.0f);
            L.SetCollisionRadius(1.0f);
            R.SetCollisionRadius(1.0f);
            L1.SetCollisionRadius(1.0f);
            R1.SetCollisionRadius(1.0f);
            L2.SetCollisionRadius(1.0f);
            R2.SetCollisionRadius(1.0f);
            L3.SetCollisionRadius(1.0f);
            R3.SetCollisionRadius(1.0f);
            m.Stats.Size.BaseValue = 10;
            L.Stats.Size.BaseValue = 10;
            R.Stats.Size.BaseValue = 10;
            L1.Stats.Size.BaseValue = 10;
            R1.Stats.Size.BaseValue = 10;
            L2.Stats.Size.BaseValue = 10;
            R2.Stats.Size.BaseValue = 10;
            L3.Stats.Size.BaseValue = 10;
            R3.Stats.Size.BaseValue = 10;
            CreateTimer(5.0f, () =>
            {
                m.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                L.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                R.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                L1.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                R1.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                L2.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                R2.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                L3.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                R3.TakeDamage(owner, 50000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            });
        }

        public void OnUpdate(float diff)
        {
        }
    }
}