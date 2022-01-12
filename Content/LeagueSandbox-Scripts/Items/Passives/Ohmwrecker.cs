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
    public class ItemID_3056 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner)
        {
        }

        public void OnUpdate(float diff)
        {
            foreach (var enemy in GetUnitsInRange(_owner.Position, 850, true))
            {
                if (enemy.Team != _owner.Team)
                {
                    if (enemy is IBaseTurret)
                    {
                        if (!_owner.HasBuff("RaptorCloakMS"))
                        {
                            AddBuff("RaptorCloakMS", 1f, 1, _owner.GetSpell(0), _owner, _owner);
                            LogDebug("applied");
                        }
                    }
                }
            }
        }
    }
}