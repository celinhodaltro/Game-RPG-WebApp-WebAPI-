﻿using System.Reflection;
using Autofac;
using Game.Common.Contracts;
using Game.Common.Contracts.Chats;
using Game.Common.Contracts.Creatures;
using Game.Common.Contracts.Items;
using Server.Events.Creature;
using Server.Events.Subscribers;

namespace Server.Standalone.IoC.Modules;

public static class EventInjection
{
    public static ContainerBuilder AddEvents(this ContainerBuilder builder)
    {
        builder.RegisterServerEvents();
        builder.RegisterGameEvents();
        builder.RegisterEventSubscribers();
        builder.RegisterType<EventSubscriber>().SingleInstance();
        builder.RegisterType<FactoryEventSubscriber>().SingleInstance();

        return builder;
    }

    private static void RegisterServerEvents(this ContainerBuilder builder)
    {
        var assembly = Assembly.GetAssembly(typeof(CreatureAddedOnMapEventHandler));
        builder.RegisterAssemblyTypes(assembly);
    }

    private static void RegisterGameEvents(this ContainerBuilder builder)
    {
        builder.RegisterAssembliesByInterface(typeof(IGameEventHandler));
    }

    private static void RegisterEventSubscribers(this ContainerBuilder builder)
    {
        var types = Container.AssemblyCache;
        builder.RegisterAssemblyTypes(types).As<ICreatureEventSubscriber>().SingleInstance();
        builder.RegisterAssemblyTypes(types).As<IItemEventSubscriber>().SingleInstance();
        builder.RegisterAssemblyTypes(types).As<IChatChannelEventSubscriber>().SingleInstance();
    }
}