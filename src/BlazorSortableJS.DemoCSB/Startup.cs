using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Builder;
using DemoCore;

namespace BlazorSortableJS.Demo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
       
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
