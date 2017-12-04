﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)
using OpenNos.Core;
using OpenNos.Core.Serializing;

namespace OpenNos.GameObject.Packets.ClientPackets
{
    [PacketHeader("ncif")]
    public class NcifPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public byte Type { get; set; }

        [PacketIndex(1)]
        public long TargetId { get; set; }
        
        #endregion
    }
}