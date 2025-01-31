using System;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApplication13.Filters
{
    /// <summary>
    /// This is a rate limit attribute 
    /// that can be used to limit the number of requests 
    /// that can be made to a specific action in a specific time window.
    /// Make sure to <c>not</c> use it if your application running on more than one server and consider to use a distributed cache instead.
    /// </summary>
    public class CustomRateLimitAttribute : ActionFilterAttribute
    {
        private int _limit = 10; // Allowed Request Count.
        private TimeSpan _window = TimeSpan.FromMinutes(1); // Time Window
        private static MemoryCache _cache = MemoryCache.Default;
        private object _lock = new object();
        public CustomRateLimitAttribute(int limit, int window)
        {
            _limit = limit;
            _window = TimeSpan.FromMinutes(window);
        }

        public override bool AllowMultiple
        {
            get { return false; }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var ip = HttpContext.Current.Request.UserHostAddress;
            var key = $"{ip}_{actionContext.ActionDescriptor.ControllerDescriptor.ControllerName}_{actionContext.ActionDescriptor.ActionName}";

            lock (_lock)
            {
                var count = _cache.Get(key) as int? ?? 0;
                if (count >= _limit)
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Conflict);
                    actionContext.Response.Content = new System.Net.Http.StringContent("Too many request and Rate limit exceeded.");
                    return;
                }
                _cache.Set(key, count + 1, DateTimeOffset.UtcNow.Add(_window));
            }
            base.OnActionExecuting(actionContext);
        }
    }
}