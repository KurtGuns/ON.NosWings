﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)
using OpenNos.Core;

namespace OpenNos.Handler.Packets.ServerPackets
{
    public class NcifPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte ObjectType { get; set; }

        [PacketIndex(1)]
        public long ObjectId { get; set; }

        #endregion
    }
}