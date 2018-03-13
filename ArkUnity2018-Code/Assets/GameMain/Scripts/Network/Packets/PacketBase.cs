using GameFramework.Network;



namespace ARKGame
{
    public abstract class PacketBase : Packet
    {
        public sealed override int Id { get { if (MsgHead == null) return 0; return MsgHead.unMsgID; } }
        public AFTCPClient.MsgHead MsgHead { get; set; }
    }


}