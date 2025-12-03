using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DATA.Interfaces
{
    public interface ITokenRepository
    {
        string GenerateAccessToken(int userId);
        string GenerateRefreshToken();
    }
}
