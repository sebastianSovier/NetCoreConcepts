using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace NetCoreConcepts
{


    public class DependencyFilterProvider<TFilter> : IFilterFactory where TFilter : IFilterMetadata
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<TFilter>();
        }
    }
}
