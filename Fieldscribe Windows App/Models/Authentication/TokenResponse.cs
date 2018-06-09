using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fieldscribe_Windows_App
{
    class TokenResponse
    {
        public string token_type { get; set; }

        public string access_token { get; set; }

        public int expires_in { get; set; }
    }
}
