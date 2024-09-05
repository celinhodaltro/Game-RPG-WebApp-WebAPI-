﻿using Data.Entities;
using Server.Common.Contracts.Network;

namespace Networking.Packets.Outgoing.Login;

public class CharacterListPacket : OutgoingPacket
{
    private readonly AccountEntity _accountEntity;
    private readonly string _ipAddress;
    private readonly string _serverName;

    public CharacterListPacket(AccountEntity account, string serverName, string ipAddress)
    {
        _accountEntity = account;
        _serverName = serverName;
        _ipAddress = ipAddress;
    }

    public override void WriteToMessage(INetworkMessage message)
    {
        AddCharList(message);
    }

    private void AddCharList(INetworkMessage message)
    {
        message.AddByte(0x64); //todo charlist
        message.AddByte((byte)_accountEntity.Players.Count);

        var ipAddress = ParseIpAddress(_ipAddress);

        foreach (var player in _accountEntity.Players)
        {
            if (!string.IsNullOrWhiteSpace(player.World?.Ip)) ipAddress = ParseIpAddress(player.World.Ip);

            message.AddString(player.Name);
            message.AddString(player.World?.Name ?? _serverName ?? string.Empty);

            message.AddByte(ipAddress[0]);
            message.AddByte(ipAddress[1]);
            message.AddByte(ipAddress[2]);
            message.AddByte(ipAddress[3]);

            message.AddUInt16(7172);
        }

        message.AddUInt16((ushort)_accountEntity.PremiumTime);
    }

    private static byte[] ParseIpAddress(string ip)
    {
        var localhost = new byte[] { 127, 0, 0, 1 };

        if (string.IsNullOrEmpty(ip)) return localhost;

        var parsedIp = new byte[4];

        var numbers = ip.Split(".");

        if (numbers.Length != 4) return localhost;

        var i = 0;

        foreach (var number in numbers)
        {
            if (!byte.TryParse(numbers[i], out var ipNumber)) return localhost;
            parsedIp[i++] = ipNumber;
        }

        return parsedIp;
    }
}