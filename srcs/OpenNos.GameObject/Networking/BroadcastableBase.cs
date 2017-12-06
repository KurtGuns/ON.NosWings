﻿/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NosSharp.Enums;
using OpenNos.Core;
using OpenNos.Core.Serializing;
using OpenNos.GameObject.Helpers;

namespace OpenNos.GameObject.Networking
{
    public abstract class BroadcastableBase : IDisposable
    {
        #region Members

        /// <summary>
        /// List of all connected clients.
        /// </summary>
        private readonly ConcurrentDictionary<long, ClientSession> _sessions;

        private bool _disposed;

        #endregion

        #region Instantiation

        protected BroadcastableBase()
        {
            LastUnregister = DateTime.Now.AddMinutes(-1);
            _sessions = new ConcurrentDictionary<long, ClientSession>();
        }

        #endregion

        #region Properties

        public IEnumerable<ClientSession> Sessions
        {
            get
            {
                return _sessions.Select(s => s.Value).Where(s => s.HasSelectedCharacter && !s.IsDisposing && s.IsConnected);
            }
        }

        // TO DO : OPTIMIZE
        public IEnumerable<Mate> Mates
        {
            get
            {
                ConcurrentBag<Mate> mate = new ConcurrentBag<Mate>();
                Sessions.ToList().ForEach(s => s.Character.Mates.Where(m => m.IsTeamMember).ToList().ForEach(m => mate.Add(m)));
                return mate;
            }
        }

        protected DateTime LastUnregister { get; private set; }

        #endregion

        #region Methods

        public void Broadcast(string packet)
        {
            Broadcast(null, packet);
        }

        public void Broadcast(string packet, int xRangeCoordinate, int yRangeCoordinate)
        {
            Broadcast(new BroadcastPacket(null, packet, ReceiverType.AllInRange, xCoordinate: xRangeCoordinate, yCoordinate: yRangeCoordinate));
        }

        public void Broadcast(PacketDefinition packet)
        {
            Broadcast(null, packet);
        }

        public void Broadcast(PacketDefinition packet, int xRangeCoordinate, int yRangeCoordinate)
        {
            Broadcast(new BroadcastPacket(null, PacketFactory.Serialize(packet), ReceiverType.AllInRange, xCoordinate: xRangeCoordinate, yCoordinate: yRangeCoordinate));
        }

        public void Broadcast(ClientSession client, PacketDefinition packet, ReceiverType receiver = ReceiverType.All, string characterName = "", long characterId = -1)
        {
            Broadcast(client, PacketFactory.Serialize(packet), receiver, characterName, characterId);
        }

        public void Broadcast(BroadcastPacket packet)
        {
            try
            {
                SpreadBroadcastpacket(packet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void Broadcast(ClientSession client, string content, ReceiverType receiver = ReceiverType.All, string characterName = "", long characterId = -1)
        {
            try
            {
                SpreadBroadcastpacket(new BroadcastPacket(client, content, receiver, characterName, characterId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public virtual void Dispose()
        {
            if (!_disposed)
            {
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }

        public ClientSession GetSessionByCharacterId(long characterId)
        {
            return _sessions.ContainsKey(characterId) ? _sessions[characterId] : null;
        }

        public Mate GetMateByMateTransportId(long mateTransportId)
        {
            return Mates.FirstOrDefault(m => m.MateTransportId == mateTransportId);
        }

        public void RegisterSession(ClientSession session)
        {
            if (!session.HasSelectedCharacter)
            {
                return;
            }
            session.RegisterTime = DateTime.Now;

            // Create a ChatClient and store it in a collection
            _sessions[session.Character.CharacterId] = session;
            if (session.HasCurrentMapInstance)
            {
                session.CurrentMapInstance.IsSleeping = false;
            }
            Console.Title = string.Format(Language.Instance.GetMessageFromKey("WORLD_SERVER_CONSOLE_TITLE"), ServerManager.Instance.ChannelId, ServerManager.Instance.Sessions.Count(), ServerManager.Instance.IpAddress, ServerManager.Instance.Port);
        }

        public void UnregisterSession(long characterId)
        {
            // Remove client from online clients list
            if (!_sessions.TryRemove(characterId, out ClientSession session))
            {
                return;
            }
            if (session.HasCurrentMapInstance && _sessions.Count == 0)
            {
                session.CurrentMapInstance.IsSleeping = true;
            }
            Console.Title = string.Format(Language.Instance.GetMessageFromKey("WORLD_SERVER_CONSOLE_TITLE"), ServerManager.Instance.ChannelId, ServerManager.Instance.Sessions.Count(), ServerManager.Instance.IpAddress, ServerManager.Instance.Port);
            LastUnregister = DateTime.Now;
        }

        private void SpreadBroadcastpacket(BroadcastPacket sentPacket)
        {
            if (Sessions == null || string.IsNullOrEmpty(sentPacket?.Packet))
            {
                return;
            }
            switch (sentPacket.Receiver)
            {
                case ReceiverType.All: // send packet to everyone
                    if (sentPacket.Packet.StartsWith("out"))
                    {
                        foreach (ClientSession session in Sessions)
                        {
                            if (!session.HasSelectedCharacter)
                            {
                                continue;
                            }
                            if (sentPacket.Sender != null)
                            {
                                if (!sentPacket.Sender.Character.IsBlockedByCharacter(session.Character.CharacterId))
                                {
                                    session.SendPacket(sentPacket.Packet);
                                }
                            }
                            else
                            {
                                session.SendPacket(sentPacket.Packet);
                            }
                        }
                    }
                    else
                    {
                        Parallel.ForEach(Sessions, session =>
                        {
                            if (!session.HasSelectedCharacter)
                            {
                                return;
                            }
                            if (sentPacket.Sender != null)
                            {
                                if (!sentPacket.Sender.Character.IsBlockedByCharacter(session.Character.CharacterId))
                                {
                                    session.SendPacket(sentPacket.Packet);
                                }
                            }
                            else
                            {
                                session.SendPacket(sentPacket.Packet);
                            }
                        });
                    }
                    break;
                case ReceiverType.AllExceptMeAct4:
                    if (sentPacket.Sender == null)
                    {
                        return;
                    }
                    foreach (ClientSession session in Sessions.Where(s =>
                        s.SessionId != sentPacket.Sender.SessionId && s.Character.Faction == sentPacket.Sender.Character.Faction && s.HasSelectedCharacter))
                    {
                        session.SendPacket(sentPacket.Packet);
                    }
                    break;
                case ReceiverType.AllExceptMe: // send to everyone except the sender
                    if (sentPacket.Packet.StartsWith("out"))
                    {

                        foreach (ClientSession session in Sessions.Where(s => s.SessionId != sentPacket.Sender.SessionId))
                        {
                            if (!session.HasSelectedCharacter)
                            {
                                continue;
                            }
                            if (sentPacket.Sender != null)
                            {
                                if (!sentPacket.Sender.Character.IsBlockedByCharacter(session.Character.CharacterId))
                                {
                                    session.SendPacket(sentPacket.Packet);
                                }
                            }
                            else
                            {
                                session.SendPacket(sentPacket.Packet);
                            }
                        }
                    }
                    else
                    {
                        Parallel.ForEach(Sessions.Where(s => s.SessionId != sentPacket.Sender.SessionId), session =>
                        {
                            if (!session.HasSelectedCharacter)
                            {
                                return;
                            }
                            if (sentPacket.Sender != null)
                            {
                                if (!sentPacket.Sender.Character.IsBlockedByCharacter(session.Character.CharacterId))
                                {
                                    session.SendPacket(sentPacket.Packet);
                                }
                            }
                            else
                            {
                                session.SendPacket(sentPacket.Packet);
                            }
                        });
                    }
                    break;
                case ReceiverType.AllExceptGroup:
                    foreach (ClientSession session in Sessions.Where(s => s.SessionId != sentPacket.Sender.SessionId && (s.Character?.Group == null || s.Character?.Group?.GroupId != sentPacket.Sender?.Character?.Group?.GroupId)))
                    {
                        if (!session.HasSelectedCharacter)
                        {
                            continue;
                        }
                        if (sentPacket.Sender != null)
                        {
                            if (!sentPacket.Sender.Character.IsBlockedByCharacter(session.Character.CharacterId))
                            {
                                session.SendPacket(sentPacket.Packet);
                            }
                        }
                        else
                        {
                            session.SendPacket(sentPacket.Packet);
                        }
                    }
                    break;
                case ReceiverType.AllInRange: // send to everyone which is in a range of 50x50
                    if (sentPacket.XCoordinate != 0 && sentPacket.YCoordinate != 0)
                    {
                        Parallel.ForEach(Sessions.Where(s => s.Character.IsInRange(sentPacket.XCoordinate, sentPacket.YCoordinate)), session =>
                        {
                            if (!session.HasSelectedCharacter)
                            {
                                return;
                            }
                            if (sentPacket.Sender != null)
                            {
                                if (!sentPacket.Sender.Character.IsBlockedByCharacter(session.Character.CharacterId))
                                {
                                    session.SendPacket(sentPacket.Packet);
                                }
                            }
                            else
                            {
                                session.SendPacket(sentPacket.Packet);
                            }
                        });
                    }
                    break;

                case ReceiverType.OnlySomeone:
                    if (sentPacket.SomeonesCharacterId > 0 || !string.IsNullOrEmpty(sentPacket.SomeonesCharacterName))
                    {
                        ClientSession targetSession = Sessions.SingleOrDefault(s => s.Character.CharacterId == sentPacket.SomeonesCharacterId || s.Character.Name == sentPacket.SomeonesCharacterName);
                        if (targetSession != null && targetSession.HasSelectedCharacter)
                        {
                            if (sentPacket.Sender != null)
                            {
                                if (!sentPacket.Sender.Character.IsBlockedByCharacter(targetSession.Character.CharacterId))
                                {
                                    targetSession.SendPacket(sentPacket.Packet);
                                }
                                else
                                {
                                    sentPacket.Sender.SendPacket(UserInterfaceHelper.Instance.GenerateInfo(Language.Instance.GetMessageFromKey("BLACKLIST_BLOCKED")));
                                }
                            }
                            else
                            {
                                targetSession.SendPacket(sentPacket.Packet);
                            }
                        }
                    }
                    break;

                case ReceiverType.AllNoEmoBlocked:
                    Parallel.ForEach(Sessions.Where(s => !s.Character.EmoticonsBlocked), session =>
                    {
                        if (!session.HasSelectedCharacter)
                        {
                            return;
                        }
                        if (!sentPacket.Sender.Character.IsBlockedByCharacter(session.Character.CharacterId))
                        {
                            session.SendPacket(sentPacket.Packet);
                        }
                    });
                    break;

                case ReceiverType.AllNoHeroBlocked:
                    Parallel.ForEach(Sessions.Where(s => !s.Character.HeroChatBlocked), session =>
                    {
                        if (!session.HasSelectedCharacter)
                        {
                            return;
                        }
                        if (!sentPacket.Sender.Character.IsBlockedByCharacter(session.Character.CharacterId))
                        {
                            session.SendPacket(sentPacket.Packet);
                        }
                    });
                    break;

                case ReceiverType.Group:
                    Parallel.ForEach(Sessions.Where(s => s.Character?.Group != null && sentPacket.Sender?.Character?.Group != null && s.Character.Group.GroupId == sentPacket.Sender.Character.Group.GroupId), session =>
                    {
                        session.SendPacket(sentPacket.Packet);
                    });
                    break;

                case ReceiverType.Unknown:
                    break;
            }
        }


        #endregion
    }
}