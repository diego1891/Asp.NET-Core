using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class MiControllerBase : ControllerBase
    {
        private IMediator mediator1;
        protected IMediator Mediator => mediator1 ?? (mediator1 = HttpContext.RequestServices.GetService<IMediator>());

    }
}