﻿using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Obvs.Configuration;
using Obvs.Types;
using IMessage = Obvs.Types.IMessage;

namespace Obvs.ActiveMQ.Configuration
{
    public static class ActiveMQConfigExtensions
    {
        public static ICanSpecifyActiveMQServiceName<TMessage, TCommand, TEvent, TRequest, TResponse> WithActiveMQEndpoints<TServiceMessage, TMessage, TCommand, TEvent, TRequest, TResponse>(this ICanAddEndpoint<TMessage, TCommand, TEvent, TRequest, TResponse> canAddEndpoint) 
            where TMessage : class
            where TServiceMessage : class 
            where TCommand : class, TMessage 
            where TEvent : class, TMessage 
            where TRequest : class, TMessage 
            where TResponse : class, TMessage
        {
            return new ActiveMQFluentConfig<TServiceMessage, TMessage, TCommand, TEvent, TRequest, TResponse>(canAddEndpoint);
        }

        public static ICanSpecifyActiveMQServiceName<IMessage, ICommand, IEvent, IRequest, IResponse> WithActiveMQEndpoints<TServiceMessage>(this ICanAddEndpoint<IMessage, ICommand, IEvent, IRequest, IResponse> canAddEndpoint) where TServiceMessage : class 
        {
            return new ActiveMQFluentConfig<TServiceMessage, IMessage, ICommand, IEvent, IRequest, IResponse>(canAddEndpoint);
        }
        
        public static ICanAddEndpointOrLoggingOrCorrelationOrCreate<TMessage, TCommand, TEvent, TRequest, TResponse> WithActiveMQSharedConnectionScope<TMessage, TCommand, TEvent, TRequest, TResponse>(this ICanAddEndpoint<TMessage, TCommand, TEvent, TRequest, TResponse> canAddEndpoint, string brokerUri,
            Func<ICanAddEndpoint<TMessage, TCommand, TEvent, TRequest, TResponse>, ICanAddEndpointOrLoggingOrCorrelationOrCreate<TMessage, TCommand, TEvent, TRequest, TResponse>> endPointFactory)
            where TMessage : class
            where TCommand : class, TMessage
            where TEvent : class, TMessage
            where TRequest : class, TMessage
            where TResponse : class, TMessage
        {
            var connectionFactory = new ConnectionFactory(brokerUri, ConnectionClientId.CreateWithSuffix("Shared"));
            ActiveMQFluentConfigContext.SharedConnection = connectionFactory.CreateConnection().GetLazyConnection();

            var result = endPointFactory(canAddEndpoint);

            ActiveMQFluentConfigContext.SharedConnection = null;
            return result;
        }

        public static ICanAddEndpointOrLoggingOrCorrelationOrCreate<IMessage, ICommand, IEvent, IRequest, IResponse> WithActiveMQSharedConnectionScope<TServiceMessage>(this ICanAddEndpoint<IMessage, ICommand, IEvent, IRequest, IResponse> canAddEndpoint, string brokerUri,
            Func<ICanAddEndpoint<IMessage, ICommand, IEvent, IRequest, IResponse>, ICanAddEndpointOrLoggingOrCorrelationOrCreate<IMessage, ICommand, IEvent, IRequest, IResponse>> endPointFactory) where TServiceMessage : class
        {
            return canAddEndpoint.WithActiveMQSharedConnectionScope(brokerUri, endPointFactory);
        }
    }
}