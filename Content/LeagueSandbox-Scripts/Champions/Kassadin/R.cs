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
    public class RiftWalk : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
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
            var owner = spell.CastInfo.Owner;
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var startPos = owner.Position;

            var to = trueCoords - startPos;
            if (to.Length() > 700f)
            {
                trueCoords = GetPointFromUnit(owner, 475f);
            }
            PlayAnimation(owner, "Spell3", 0, 0, 1);
            AddBuff("RiftWalk", 20.0f, 1, spell, owner, owner);

            AddBuff("EStacks", float.MaxValue, 1, spell, owner, owner, true);

            TeleportTo(owner, trueCoords.X, trueCoords.Y);
            AddParticle(owner, null, "Kassadin_Base_R_appear.troy", owner.Position);

            var AOEdmg = spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var buff = spell.CastInfo.Owner.GetBuffWithName("RiftWalk");
            float MANA = spell.CastInfo.Owner.Stats.ManaPoints.Total * 0.02f + (0.01f * buff.StackCount);
            float damage = 60f + 20f * spell.CastInfo.SpellLevel + MANA + (30f * spell.CastInfo.SpellLevel) * buff.StackCount;
            //TODO: Find a way to increase damage and ManaCost based on stacks
            float bonusdmg = buff.StackCount * 40;
            LogDebug(bonusdmg.ToString());
            target.TakeDamage(spell.CastInfo.Owner, damage + bonusdmg, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
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

        static internal bool unlockE = false;

        public void OnUpdate(float diff)
        {

            if (_owner.Stats.CurrentMana <= _owner.GetSpell("RiftWalk").SpellData.ManaCost[1])
            {
                SealSpellSlot(_owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
            }
            else
            {
                SealSpellSlot(_owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
            }

            if (_owner.HasBuff("EStacks")){
                //LogDebug(_owner.GetBuffWithName("EStacks").StackCount.ToString());
                if (_owner.GetBuffWithName("EStacks").StackCount == 6)
                {
                    if(unlockE == false)
                    {
                        CreateTimer((float)0.1, () => { SealSpellSlot(_owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false); });
                        unlockE = true;
                    }
                }
            }
        }
    }
}