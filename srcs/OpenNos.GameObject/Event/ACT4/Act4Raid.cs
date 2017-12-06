﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NosSharp.Enums;
using OpenNos.Core;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Map;
using OpenNos.GameObject.Networking;
using CloneExtensions;
using System.Reactive.Linq;

namespace OpenNos.GameObject.Event.ACT4
{
    public static class Act4Raid
    {

        #region Methods

        public async static void GenerateRaid(byte type, byte faction)
        {
            ScriptedInstance raid = ServerManager.Instance.Act4Raids.FirstOrDefault(r => r.Id == type);
            MapInstance lobby = ServerManager.Instance.Act4Maps.FirstOrDefault(m => m.Map.MapId == 134);

            if (raid == null || lobby == null)
            {
                Logger.Log.Error(raid == null ? $"Act4 raids is missing - type : {type}" : "There is no map in Act4Maps with MapId == 134");
                return;
            }

            ServerManager.Instance.Act4RaidStart = DateTime.Now;
            lobby.CreatePortal(new Portal()
            {
                SourceMapId = 134,
                SourceX = 139,
                SourceY = 100,
                Type = (short)(9 + faction)
            }, 3600, true);

            foreach (MapInstance map in ServerManager.Instance.Act4Maps)
            {
                map.Sessions.Where(s => s?.Character?.Faction == (FactionType)faction).ToList().ForEach(s =>
                    s.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("ACT4_RAID_OPEN"), ((Act4RaidType)type).ToString()), 0)));
            }

            foreach (Family family in ServerManager.Instance.FamilyList)
            {
                family.Act4Raid = ServerManager.Instance.Act4Raids.FirstOrDefault(r => r.Id == type).GetClone();
                family.Act4Raid.LoadScript(MapInstanceType.RaidInstance);
            }

            await Task.Delay(60 * 60 * 1000);

            foreach (Family family in ServerManager.Instance.FamilyList)
            {
                family.Act4Raid.Mapinstancedictionary.Values.ToList().ForEach(m => m.Dispose());
                family.Act4Raid = null;
            }
        }

        #endregion
    }
}
