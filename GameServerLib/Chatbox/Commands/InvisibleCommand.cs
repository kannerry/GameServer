using GameServerCore;

namespace LeagueSandbox.GameServer.Chatbox.Commands
{
    public class InvisibleCommand : ChatCommandBase
    {
        private readonly IPlayerManager _playerManager;

        public override string Command => "invisible";
        public override string Syntax => $"{Command}";

        public InvisibleCommand(ChatCommandManager chatCommandManager, Game game)
            : base(chatCommandManager, game)
        {
            _playerManager = game.PlayerManager;
        }

        public override void Execute(int userId, bool hasReceivedArguments, string arguments = "")
        {
            var swag = _playerManager.GetPlayers();
            var champ = _playerManager.GetPeerInfo(userId).Champion;
            foreach (var player in swag)
            {
                Game.PacketNotifier.NotifyModelTransparency((int)player.Item2.Champion.GetPlayerId(), champ, 0.0f, 0.2f);
            }
        }
    }
}
