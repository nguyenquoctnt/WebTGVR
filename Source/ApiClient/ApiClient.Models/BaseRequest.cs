using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class BaseRequest
    {

        private string headerUserField;

        private string headerPassField;

        private string productKeyField;

        private string languageCodeField;

        private string agentAccountField;

        private string agentPasswordField;
        private string ipRequestField;

        public string HeaderUser
        {
            get
            {
                return this.headerUserField;
            }
            set
            {
                this.headerUserField = value;
            }
        }


        public string HeaderPass
        {
            get
            {
                return this.headerPassField;
            }
            set
            {
                this.headerPassField = value;
            }
        }


        public string ProductKey
        {
            get
            {
                return this.productKeyField;
            }
            set
            {
                this.productKeyField = value;
            }
        }


        public string LanguageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }


        public string AgentAccount
        {
            get
            {
                return this.agentAccountField;
            }
            set
            {
                this.agentAccountField = value;
            }
        }
        public string AgentPassword
        {
            get
            {
                return this.agentPasswordField;
            }
            set
            {
                this.agentPasswordField = value;
            }
        }
        public string IpRequest
        {
            get
            {
                return this.ipRequestField;
            }
            set
            {
                this.ipRequestField = value;
            }
        }
    }
}
