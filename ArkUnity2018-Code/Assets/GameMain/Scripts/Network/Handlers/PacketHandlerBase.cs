using GameFramework.Network;
using System.IO;

namespace ARKGame
{
    public abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract int Id { get; }
        public abstract PacketBase DeserializePacket(Stream source);
        public abstract void Handle(object sender, Packet packet);
    }
}