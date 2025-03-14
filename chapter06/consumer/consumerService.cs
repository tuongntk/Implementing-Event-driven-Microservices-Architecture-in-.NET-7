﻿using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consumer
{
    internal class consumerService : BackgroundService, IConsumerService
    {
        private readonly IConsumer<int, string> consumer;
        private readonly string topicName;
        public consumerService(ConsumerConfig config, string topic)
        {
            consumer = new ConsumerBuilder<int, string>(config).Build();
            topicName = topic;
        }
        public async Task Receive()
        {
            await ExecuteAsync(CancellationToken.None);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                consumer.Subscribe(topicName);
                var result = consumer.Consume(stoppingToken);
                await new MessageReceivedEventHandler().Handle(result);
            }
        }
    }

    internal interface IConsumerService
    {
        Task Receive();
    }

}
