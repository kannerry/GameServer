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


// THIS IS THE BEST THATI  CAN DO
// VERRY SCUFFFFED :(

namespace Spells
{
    public class VelkozQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
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
        static internal Vector2 targetPos;
        static internal Vector2 tempPos;
        static internal Vector2 tempPosL;
        static internal Vector2 tempPosR;
        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var xmin = AddMinion(owner, "Velkoz", "temp", owner.Position, ignoreCollision: true, isVisible: false);
            var champs = GetAllChampionsInRange(owner.Position, 50000);
            foreach (var champ in champs)
            {
                xmin.SetInvisible((int)champ.GetPlayerId(), xmin, 0.0f, 0.0f);
            }
            FaceDirection(GetPointFromUnit(owner, 10), xmin);
            tempPos = GetPointFromUnit(xmin, 1100);
            tempPosL = GetPointFromUnit(xmin, 1100, 1);
            tempPosR = GetPointFromUnit(xmin, 1100, -1);
            xmin.TakeDamage(owner, 10000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_RAW, false);
            targetPos = GetPointFromUnit(owner, 1100f, 0);
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
            CreateTimer(4.0f, () => { VelkozQMissile.comeBack = false; });
            CreateTimer(1.0f, () =>
            {
                if (VelkozQMissile.cast90DEG == false)
                {
                    LogDebug("DID NOT HIT");
                    if (VelkozQMissile.comeBack == false)
                    {
                        SpellCast(owner, 0, SpellSlotType.ExtraSlots, tempPosL, tempPosL, true, tempPos);
                        SpellCast(owner, 0, SpellSlotType.ExtraSlots, tempPosR, tempPosR, true, tempPos);
                        VelkozQMissile.comeBack = true;
                    }
                }
            });
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

    public class VelkozQMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        static internal bool comeBack = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            //var xmissile = spell.CreateSpellMissile(new MissileParameters
            //{
            //    Type = MissileType.Circle,
            //});
            ApiEventManager.OnSpellHit.AddListener(this, spell, ShieldXD, true);

            //ApiEventManager.OnSpellHit.AddListener(this, xmissile.SpellOrigin, ShieldXD, true);
        }

        IAttackableUnit unitx;
        static internal bool cast90DEG = false;
        IObjAiBase _owner;
        public void ShieldXD(ISpell spell, IAttackableUnit unit, ISpellMissile mis, ISpellSector sec)
        {
            unitx = unit;
            _owner = spell.CastInfo.Owner;
            CreateTimer(5.0f, () => { unitx = null; });
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.35;
            float damage = (float)((float)(owner.Spells[0].CastInfo.SpellLevel * 30) + ap);
            unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            //min = AddMinion(owner, "Velkoz", "temp", mis.Position, ignoreCollision: true, isVisible: false);
            cast90DEG = true;
            mis.SetToRemove();
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
            if(cast90DEG == true)
            {
                var owner = _owner;
                LogDebug("yo");
                if(comeBack == false)
                {
                    FaceDirection(owner.Position, unitx);
                    CreateTimer(0.1f, () => { SpellCast(owner, 0, SpellSlotType.ExtraSlots, GetPointFromUnit(unitx, 1100, 90), GetPointFromUnit(unitx, 1100, 90), true, GetPointFromUnit(unitx, 100, 90)); });
                    CreateTimer(0.1f, () => { SpellCast(owner, 0, SpellSlotType.ExtraSlots, GetPointFromUnit(unitx, 1100, -90), GetPointFromUnit(unitx, 1100, -90), true, GetPointFromUnit(unitx, 100, -90)); });
                    comeBack = true;
                    CreateTimer(2.0f, () => { comeBack = false; });
                }
                cast90DEG = false;
            }
        }
    }
}