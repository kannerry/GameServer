using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Feast : ISpellScript
    {
        private IAttackableUnit Target;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, false);
        }

        public void OnDeath(IDeathData deathData)
        {
            int i = 0;
            LogDebug("died");
            while (i < _owner.GetBuffWithName("Feast").StackCount)
            {
                i++;
                if (i == _owner.GetBuffWithName("Feast").StackCount)
                {
                    LogDebug((i / 2).ToString());
                    RemoveStacks(i / 2);
                }
            }
        }

        public void RemoveStacks(int var)
        {
            int x = 0;
            foreach (var swag in _owner.GetBuffsWithName("Feast"))
            {
                if(x < var)
                {
                    x++;
                    swag.DeactivateBuff();
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var APratio = owner.Stats.AbilityPower.Total * 0.7f;
            var damage = 300 + spell.CastInfo.SpellLevel * 175 + APratio;
            var damageMM = 1000 + APratio;
            if(Target is IMinion || Target is IMonster)
            {
                Target.TakeDamage(owner, damageMM, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            if(Target is IChampion)
            {
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            CreateTimer(0.05f, () => 
            {
            if(Target.IsDead == true)
                {
                    add = true;
                }
            });
            AddParticleTarget(owner, Target, "feast_tar.troy", Target, 1f, 1f);
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
        bool add = false;
        public void OnUpdate(float diff)
        {
            if(add == true)
            {
                AddBuff("Feast", float.MaxValue, 1, _owner.Spells[3], _owner, _owner, true);
                add = false;
            }
        }
    }
}