using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class KhazixQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            //
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
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
            var target = spell.CastInfo.Targets[0].Unit;
            var owner = spell.CastInfo.Owner;
            AddParticleTarget(owner, target, "Khazix_Base_Q_SingleEnemy_Tar.troy", target);
            var damage = 45 + 25 * spell.CastInfo.SpellLevel;
            var ad = owner.Stats.AttackDamage.Total * 1.15f;
            if (target.HasBuff("Isolation"))
            {
                var total = damage + ad;
                target.TakeDamage(owner, total * 1.1f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                LogDebug("has isolation");
            }
            else
            {
                var total = damage + ad;
                target.TakeDamage(owner, total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }

            var Champs = GetAllChampionsInRange(owner.Position, 50000);
            foreach (IChampion player in Champs)
            {
                owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0f);
                owner.SetStatus(StatusFlags.Targetable, true);
                owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true);
            }

        }

        public void ApplyEffects(IObjAiBase owner, IAttackableUnit target, ISpell spell, ISpellMissile missile)
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
        IObjAiBase _owner;
        ISpell _spell;
        public void OnUpdate(float diff)
        {
            //LogDebug("yo");
            var c = GetChampionsInRange(_owner.Position, 2000, true);
            foreach(var champion in c)
            {
                // champions within 2000 units
                if(champion.Team != _owner.Team)
                {
                    //NOW WE ARE INSIDE OF THE CHAMPION
                    // get units within 425 units of that champion
                    var cu = GetUnitsInRange(champion.Position, 425, true);
                    bool isolation = true;
                    foreach(var unit in cu)
                    {
                        // if there is a unit within that 425 that is on the enemy team
                        if(unit.Team != _owner.Team)
                        {
                            //and if that unit is not the champion we are looking at
                            if(unit != champion)
                            {
                                // they are not ISOLATED
                                isolation = false;
                            }
                        }
                    }

                    if(isolation == true)
                    {
                        AddBuff("Isolation", 0.1f, 1, _spell, champion, _owner);
                    }

                }
            }
        }
    }
}