using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Clients.Webhook
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public int ErrorCode { get; set; }
        public string Content { get; set; }
    }
}
