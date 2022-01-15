using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class AatroxPassive : ICharScript
    {
        private ISpell originspell;
        private IObjAiBase _owner;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {

            owner.Stats.CurrentMana = 1;

            originspell = spell;
            _owner = owner;
            ApiEventManager.OnTakeDamage.AddListener(this, owner, TargetExecute, false);
        }
        bool haspassive = false;
        bool timer = false;
        bool oldpos = false;
        Vector2 oldposV;
        private void TargetExecute(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            if(haspassive == true)
            {
                if (unit1.Stats.CurrentHealth < unit1.Stats.HealthPoints.Total * 0.05f)
                {
                    var unit1champ = unit1 as IChampion;
                    if (timer != true)
                    {
                        timer = true;
                        oldpos = true;
                        unit1champ.Respawn();
                        unit1.Stats.CurrentMana -= unit1.Stats.ManaPoints.Total - 1;
                        TeleportTo(unit1champ, oldposV.X, oldposV.Y);
                        //AddBuff("ZhonyasHourglassBuff", 4.0f, 1, unit1champ.GetSpell(0), unit1, unit1champ);
                        //AddParticle(_owner, _owner, "GuardianAngel_tar.troy", _owner.Position);

                        AddBuff("AatroxPassiveDebuff", 3.0f, 1, unit1champ.GetSpell(0), unit1, unit1champ);

                        var hpval = unit1champ.Stats.HealthPoints.Total * 0.7f;
                        unit1champ.TakeDamage(_owner, hpval, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                        CreateTimer(160f, () => { timer = false; });
                    }
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            if (oldpos == false)
            {
                oldposV = _owner.Position;
            }
            if (oldpos == true)
            {
                oldpos = false;
            }
            if (_owner.Stats.CurrentMana == _owner.Stats.ManaPoints.Total)
            {
                if (haspassive != true)
                {
                    haspassive = true;
                }
            }
        }
    }
}