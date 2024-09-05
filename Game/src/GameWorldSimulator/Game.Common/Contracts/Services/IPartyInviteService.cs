﻿using Game.Common.Contracts.Creatures;

namespace Game.Common.Contracts.Services;

public interface IPartyInviteService
{
    void Invite(IPlayer player, IPlayer invited);
}