﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core.Serializing;
using OpenNos.Domain;

namespace OpenNos.GameObject.Packets.CommandPackets
{
    [PacketHeader("$BlockExp", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class BlockExpPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public string CharacterName { get; set; }

        [PacketIndex(1)]
        public int Duration { get; set; }

        [PacketIndex(2, SerializeToEnd = true)]
        public string Reason { get; set; }

        public static string ReturnHelp()
        {
            return "$BlockExp CHARACTERNAME DURATION REASON";
        }

        public override string ToString()
        {
            return $"BlockExp Command CharacterName: {CharacterName} Duration: {Duration} Reason: {Reason}";
        }

        #endregion
    }
}