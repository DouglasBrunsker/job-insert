using Brunsker.Integracao.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Brunsker.Integracao.WorkService.JOB.Controllers
{
    [Route("job/[controller]")]
    [ApiController]
    public class ExecutarJob : ControllerBase
    {
        private readonly IExecutarService executarService;
        private readonly ILogger<ExecutarJob> _logger;
        private Timer _timer;

        public ExecutarJob(ILogger<ExecutarJob> logger, IExecutarService executarService)
        {
            _logger = logger;
            this.executarService = executarService ?? throw new ArgumentNullException(nameof(executarService));
        }

        [HttpPut("Start")]
        public async Task StartAsync()
        {
            _logger.LogInformation("Inializando job, intervado de execução:  " + "Intervalo em milissegundos: {@tempo}", new { tempo = 60000 });

            _timer = new Timer(Callback, null, 60000, Timeout.Infinite);

            _logger.LogInformation("Job Iniciado com Sucesso  ", null);

            await Task.CompletedTask;

        }
        private void Callback(Object state)
        {
            executarService.ExecutarProcessoAsync();
            // Long running operation
            _timer.Change(60000, Timeout.Infinite);
        }
    }
}
