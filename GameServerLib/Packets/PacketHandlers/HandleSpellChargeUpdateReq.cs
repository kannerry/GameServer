using GameServerCore.Packets.Handlers;
using GameServerCore.Packets.PacketDefinitions.Requests;
using System.Numerics;

namespace LeagueSandbox.GameServer.Packets.PacketHandlers
{
    public class HandleSpellChargeUpdateReq : PacketHandlerBase<SpellChargeUpdateReq>
    {
        private readonly Game _game;

        public HandleSpellChargeUpdateReq(Game game)
        {
            _game = game;
        }

        public override bool HandlePacket(int userId, SpellChargeUpdateReq req)
        {
            // TODO: Implement handling for this request.
            //_game.PacketNotifier.NotifyDebugMessage($"X: {req.Position.X} Y: {req.Position.Y} Z: {req.Position.Z}");
            foreach (var champ in _game.ObjectManager.GetAllChampions())
            {
                if(champ.GetPlayerId() == userId)
                {
                    //_game.PacketNotifier.NotifyDebugMessage(champ.Model);
                    champ.SpellChargeXY = new Vector2(req.Position.X, req.Position.Z);
                    //_game.PacketNotifier.NotifyDebugMessage($"X: {champ.SpellChargeXYZ.X} Y: {champ.SpellChargeXYZ.Y} Z: {champ.SpellChargeXYZ.Z}");
                }
            }

            return true;
        }
    }
}
