using System.IO;
using System.Web.Hosting;

namespace VDMMutiline.Core.Mvc.ViewEngines
{
    sealed class UserAgentWorkerRequest : SimpleWorkerRequest
    {
        private readonly string _userAgent;

        public UserAgentWorkerRequest(string userAgent)
            : base(string.Empty, string.Empty, new StringWriter())
        {
            this._userAgent = userAgent;
        }

        public override string GetKnownRequestHeader(int index)
        {
            return (index != 0x27) ? null : this._userAgent;
        }
    }
}
