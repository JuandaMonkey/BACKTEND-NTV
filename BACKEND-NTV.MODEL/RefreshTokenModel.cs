using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.MODEL
{
    public class RefreshTokenModel
    {
        public int Cliente_Id { get; set; }
        public string Token { get; set; }
        public DateTime Expira { get; set; }
    }

}
