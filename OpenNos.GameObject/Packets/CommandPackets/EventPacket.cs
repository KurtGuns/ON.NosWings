﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject
{
    [PacketHeader("$Event", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class EventPacket : PacketDefinition
    {

        [PacketIndex(0)]
        public EventType EventType { get; set; }

    }
}