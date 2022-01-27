using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3153 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase _owner;

        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            var dmg = Unit.Stats.CurrentHealth * 0.08f;
            Unit.TakeDamage(_owner, dmg, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);

        }


        public void OnUpdate(float diff)
        {
        }
        IParticle p;
        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner;

            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, owner);
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            p.SetToRemove();
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
    }
}