using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class LuluPassive : ICharScript
    {
        private ISpell originspell;
        private IObjAiBase ownermain;
        static internal IMinion pix;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            originspell = spell;
            ownermain = owner;
            CreateTimer(1.0f, () => 
            { 
                pix = AddMinion(owner, "LuluFaerie", "pix", owner.Position, targetable: false);
                pix.SetStatus(StatusFlags.Ghosted, true);
                pix.SetCollisionRadius(0.000001f);
            });
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        float i;
        public void OnUpdate(float diff)
        {
            i += diff;
            if (i > 10)
            {
                i = 0;
                if (pix != null)
                {
                    if (Spells.LuluE.ETarget != null)
                    {
                        ForceMovement(pix, "RUN", GetPointFromUnit(Spells.LuluE.ETarget, -125, 30), 1000, 0, 0, 0);
                    }
                    else
                    {
                        ForceMovement(pix, "RUN", GetPointFromUnit(ownermain, -125, 30), 1000, 0, 0, 0);
                    }
                }
            }

        }
    }
}