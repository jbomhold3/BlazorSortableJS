
using DemoCore;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;

namespace BlazorSortableJS.Demo
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);


            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }
    }
}
