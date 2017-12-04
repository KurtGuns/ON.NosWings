﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using System.Collections.Generic;
using OpenNos.Core.Serializing;

namespace OpenNos.GameObject.Packets.ClientPackets
{
    [PacketHeader("mtlist")]
    public class MultiTargetListPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public byte TargetsAmount { get; set; }

        [PacketIndex(1, RemoveSeparator = true)]
        public List<MultiTargetListSubPacket> Targets { get; set; }

        #endregion
    }
}