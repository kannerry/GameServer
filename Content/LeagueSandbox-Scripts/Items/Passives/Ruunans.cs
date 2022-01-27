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
    public class ItemID_3085 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase _owner;

        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            int atk = 0;
            var closest = GetUnitsInRange(_owner.Position, 600, true);
            var dmg = 10 + _owner.Stats.AttackDamage.Total * 0.5f;
            foreach(var unit in closest)
            {
                if(unit.Team != _owner.Team)
                {
                    if(atk < 2)
                    {
                        atk++;
                        unit.TakeDamage(_owner, dmg, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        var x = AddParticle(p, p, "volibear_R_chain_lighting_01.troy", unit.Position, size: 0.1f);
                    }
                }
            }
        }


        public void OnUpdate(float diff)
        {
        }
        IParticle p;
        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner;
            p = AddParticleTarget(owner, owner, "ItemHurricaneAnchor.troy", owner, lifetime: 1000f);

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