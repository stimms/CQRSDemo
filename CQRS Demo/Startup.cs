using DapperExtensions.Mapper;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CQRS_Demo.Startup))]
namespace CQRS_Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);
        }
    }
}
