using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class Camouflage : ICharScript
    {
        private ISpell originspell;
        private IObjAiBase yi;
        float stanceTime = 500;
        float stillTime = 0;
        bool beginStance = false;
        bool stance = false;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            yi = owner;
            originspell = spell;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        static internal bool isinvis = false;

        static internal float timer = 0;

        public void OnUpdate(float diff)
        {
            if (yi != null)
            {
                var Champs = GetChampionsInRange(yi.Position, 50000, true);
                if (isinvis == true)
                {
                    foreach(var player in Champs)
                    {
                        if (player.Team.Equals(yi.Team))
                        {
                            yi.SetInvisible((int)player.GetPlayerId(), yi, 0.5f, 0.1f);
                        }
                        else
                        {
                            yi.SetInvisible((int)player.GetPlayerId(), yi, 0f, 0.1f);
                            yi.SetHealthbarVisibility((int)player.GetPlayerId(), yi, false);
                        }
                    }
                    yi.SetStatus(StatusFlags.Targetable, false);
                }
                else
                {
                    foreach (var player in Champs)
                    {
                        yi.SetInvisible((int)player.GetPlayerId(), yi, 1f, 0.1f);
                        yi.SetHealthbarVisibility((int)player.GetPlayerId(), yi, true);
                    }
                    yi.SetStatus(StatusFlags.Targetable, true);
                }

                // not moving
                if (yi.CurrentWaypoint.Value == yi.Position)
                {
                    if(isinvis == false)
                    {
                        if(timer < 4000)
                        {
                            timer += diff;
                        }
                        if(timer >= 4000)
                        {
                            isinvis = true;
                        }
                    }
                }
                else
                {
                    if (isinvis == true)
                    {
                        isinvis = false;
                        timer = 0;
                    }
                }
            }

        }
    }
}