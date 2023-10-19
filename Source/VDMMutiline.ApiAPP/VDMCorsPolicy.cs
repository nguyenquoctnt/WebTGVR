

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace VDMMutiline.ApiAPP
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class VDMCorsPolicy : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;
        public VDMCorsPolicy()
        {
            _policy = new CorsPolicy { AllowAnyMethod = true, AllowAnyHeader = true, AllowAnyOrigin = true };
        }
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}