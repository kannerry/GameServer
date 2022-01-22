using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class Focus : ICharScript
    {
        private ISpell originspell;
        private IObjAiBase ownermain;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            originspell = spell;
            ownermain = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            var point = GetPointFromUnit(ownermain, 900);
            var champs = GetChampionsInRange(point, 900, true);
            foreach (var unit in champs)
            {
                if (unit.Team != ownermain.Team)
                {
                    if (unit.IsVisibleByTeam(ownermain.Team)) 
                    { 
                        AddBuff("PassiveMSVayne", 0.1f, 1, originspell, ownermain, originspell.CastInfo.Owner); 
                    }
                }
            }

        }
    }
}