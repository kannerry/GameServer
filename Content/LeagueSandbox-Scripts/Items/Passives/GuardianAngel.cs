using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3026 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IParticle hasga;
        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner;
            hasga = AddParticle(owner, owner, "RighteousFuryHalo_buf.troy", owner.Position, lifetime: float.MaxValue, bone: "head");
            ApiEventManager.OnTakeDamage.AddListener(this, owner, TargetExecute, false);
        }
        bool timer = false;
        private void TargetExecute(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            if (unit1.Stats.CurrentHealth < unit1.Stats.HealthPoints.Total * 0.05f)
            {
                var unit1champ = unit1 as IChampion;
                if (timer != true)
                {
                    timer = true;
                    oldpos = true;
                    unit1champ.Respawn();
                    TeleportTo(unit1champ, oldposV.X, oldposV.Y);
                    AddBuff("ZhonyasHourglassBuff", 4.0f, 1, unit1champ.GetSpell(0), unit1, unit1champ);
                    AddParticle(_owner, _owner, "GuardianAngel_tar.troy", _owner.Position);
                    var hpval = unit1champ.Stats.HealthPoints.Total * 0.6f;
                    unit1champ.TakeDamage(_owner, hpval, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                    hasga.SetToRemove();
                    CreateTimer(300.0f, () => { timer = false; hasga = AddParticle(_owner, _owner, "RighteousFuryHalo_buf.troy", _owner.Position, lifetime: float.MaxValue, bone: "head"); });
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }
        bool oldpos = false;
        Vector2 oldposV;
        IObjAiBase _owner;
        public void OnUpdate(float diff)
        {
            if(oldpos == false)
            {
                oldposV = _owner.Position;
            }
            if(oldpos == true)
            {
                oldpos = false;
            }
        }
    }
}