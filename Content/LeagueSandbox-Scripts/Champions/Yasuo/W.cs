using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class YasuoWMovingWall : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var spellLvl = spell.CastInfo.SpellLevel;
            owner.StopMovement();
            FaceDirection(spellPos, owner);
            var x = GetPointFromUnit(owner, 250);
            var x1 = GetPointFromUnit(owner, 260);
            var x2 = GetPointFromUnit(owner, 1);
            var mushroom2 = AddMinion(owner, "TeemoMushroom", "TeemoMushroom", x, ignoreCollision: true, targetable: false);             //
            mushroom2.SetStatus(StatusFlags.Ghosted, true);                                                               //FOR Particle
            var mushroom = AddMinion(owner, "TeemoMushroom", "TeemoMushroom", x, ignoreCollision: true, targetable: false);  //MIDDLE MUSHROOM

            mushroom.SetStatus(StatusFlags.Ghosted, true);
            Vector2 LPos1 = GetPointFromUnit(owner, 3050, 30);
            Vector2 RPos1 = GetPointFromUnit(owner, 3050, 30);

            Vector2 LPos1L = GetPointFromUnit(owner, 3050, 30);
            Vector2 RPos1L = GetPointFromUnit(owner, 3050, 30);

            //var LPos1 = GetPointFromUnit(owner, 300, 30);                       //LEVEL 1
            //var RPos1 = GetPointFromUnit(owner, 300, -30);                     //LEVEL 1
            if (spellLvl == 1)
            {
                LPos1 = GetPointFromUnit(owner, 300, 30);
                RPos1 = GetPointFromUnit(owner, 300, -30);

                LPos1L = GetPointFromUnit(owner, 300, 15);
                RPos1L = GetPointFromUnit(owner, 300, -15);
            }
            if (spellLvl == 2)
            {
                LPos1 = GetPointFromUnit(owner, 350, 37);
                RPos1 = GetPointFromUnit(owner, 350, -37);

                LPos1L = GetPointFromUnit(owner, 320, 20);
                RPos1L = GetPointFromUnit(owner, 320, -20);
            }
            if (spellLvl == 3)
            {
                LPos1 = GetPointFromUnit(owner, 360, 40);
                RPos1 = GetPointFromUnit(owner, 360, -40);

                LPos1L = GetPointFromUnit(owner, 320, 20);
                RPos1L = GetPointFromUnit(owner, 320, -20);
            }
            if (spellLvl == 4)
            {
                LPos1 = GetPointFromUnit(owner, 400, 47);
                RPos1 = GetPointFromUnit(owner, 400, -47);

                LPos1L = GetPointFromUnit(owner, 320, 25);
                RPos1L = GetPointFromUnit(owner, 320, -25);
            }
            if (spellLvl == 5)
            {
                LPos1 = GetPointFromUnit(owner, 440, 50);
                RPos1 = GetPointFromUnit(owner, 440, -50);

                LPos1L = GetPointFromUnit(owner, 300, 37);
                RPos1L = GetPointFromUnit(owner, 300, -37);
            }
            var Lmushroom = AddMinion(owner, "TeemoMushroom", "TeemoMushroom", LPos1, ignoreCollision: true, targetable: false);
            var Rmushroom = AddMinion(owner, "TeemoMushroom", "TeemoMushroom", RPos1, ignoreCollision: true, targetable: false);

            var LLmushroom = AddMinion(owner, "TeemoMushroom", "TeemoMushroom", LPos1L, ignoreCollision: true, targetable: false);
            var LRmushroom = AddMinion(owner, "TeemoMushroom", "TeemoMushroom", RPos1L, ignoreCollision: true, targetable: false);

            Rmushroom.SetStatus(StatusFlags.NoRender, true);
            Lmushroom.SetStatus(StatusFlags.NoRender, true);
            LRmushroom.SetStatus(StatusFlags.NoRender, true);
            LLmushroom.SetStatus(StatusFlags.NoRender, true);
            mushroom.SetStatus(StatusFlags.NoRender, true);

            mushroom.Stats.HealthPoints.PercentBonus += 200f;
            mushroom2.Stats.HealthPoints.PercentBonus += 200f;
            Rmushroom.Stats.HealthPoints.PercentBonus += 200f;
            Lmushroom.Stats.HealthPoints.PercentBonus += 200f;
            LRmushroom.Stats.HealthPoints.PercentBonus += 200f;
            LLmushroom.Stats.HealthPoints.PercentBonus += 200f;
            mushroom.Stats.CurrentHealth += float.MaxValue;
            mushroom2.Stats.CurrentHealth += float.MaxValue;
            Rmushroom.Stats.CurrentHealth += float.MaxValue;
            Lmushroom.Stats.CurrentHealth += float.MaxValue;
            LRmushroom.Stats.CurrentHealth += float.MaxValue;
            LLmushroom.Stats.CurrentHealth += float.MaxValue;

            //mushroom2.SetStatus(StatusFlags.NoRender, true); (particle)

            var Champs = GetAllChampionsInRange(owner.Position, 50000);

            foreach (IChampion player in Champs)
            {
                mushroom2.SetInvisible((int)player.GetPlayerId(), mushroom2, 0f, 0.1f);
                mushroom2.SetHealthbarVisibility((int)player.GetPlayerId(), mushroom2, false);
            }
            CreateTimer(4.0f, () =>
        {
            mushroom.TakeDamage(mushroom, 500000000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            Lmushroom.TakeDamage(mushroom, 50000000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            Rmushroom.TakeDamage(mushroom, 5000000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            LLmushroom.TakeDamage(mushroom, 50000000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
            LRmushroom.TakeDamage(mushroom, 5000000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
        });
            CreateTimer(4.0f, () => { mushroom2.TakeDamage(mushroom, 500000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false); });
            FaceDirection(x1, mushroom2);
            var y = AddParticle(owner, mushroom2, "Yasuo_Base_W_windwall" + spellLvl + ".troy", x, lifetime: 4.0f, flags: FXFlags.GivenDirection, direction: new Vector3(owner.Direction.X, owner.Direction.Y, owner.Direction.Z), followGroundTilt: true);
            LogDebug(owner.Direction.ToString());
        }

        public void WallHit(IAttackableUnit unit, IAttackableUnit unit1)
        {
            LogDebug("hit");
            LogDebug(unit.Team.ToString());
            LogDebug(unit1.Team.ToString());
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
        }
    }
}