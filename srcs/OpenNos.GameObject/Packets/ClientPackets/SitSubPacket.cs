﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core.Serializing;

namespace OpenNos.GameObject.Packets.ClientPackets
{
    [PacketHeader("sit_sub_packet")] // header will be ignored for serializing just sub list packets
    public class SitSubPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public byte UserType { get; set; }

        [PacketIndex(1)]
        public long UserId { get; set; }
    }
}