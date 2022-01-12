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
    public class ItemID_3068 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase _owner;
        IParticle p;
        public void OnActivate(IObjAiBase owner)
        {
            p = AddParticleTarget(owner, owner, "Global_Item_SunfireCape_Aura.troy", owner as IGameObject, lifetime: float.MaxValue);
            _owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            p.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
            foreach (var enemy in GetUnitsInRange(_owner.Position, 425, true))
            {
                if (enemy.Team != _owner.Team)
                {
                    if (!enemy.HasBuff("SunfireCapeDamage"))
                    {
                        AddBuff("SunfireCapeDamage", 1f, 1, _owner.GetSpell(0), enemy, _owner);
                        LogDebug("applied");
                    }
                }
            }
        }
    }
}