﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http.Cors;
namespace VDMMutiline.APIExport
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ESCorsPolicy : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;
        public ESCorsPolicy()
        {
            _policy = new CorsPolicy { AllowAnyMethod = true, AllowAnyHeader = true };
            _policy.AllowAnyOrigin = true;
        }
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }

}