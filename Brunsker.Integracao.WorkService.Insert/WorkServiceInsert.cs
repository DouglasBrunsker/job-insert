using Brunsker.Integracao.Domain.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Brunsker.Integracao.WorkService.Insert
{
    public class WorkServiceInsert :
          IHostedService, IDisposable
    {
        private readonly IExecutarService service;

        Timer _timer;

        public WorkServiceInsert(IExecutarService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_timer = new Timer(Callback, null, 2000, Timeout.Infinite);

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {

            Thread.Sleep(1000);
            return Task.CompletedTask;
        }

        private void Callback(Object state)
        {
            service.ExecutarProcessoAsync();
            // Long running operation
            _timer.Change(2000, Timeout.Infinite);
        }
    }
}
