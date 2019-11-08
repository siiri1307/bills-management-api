using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Data
{
    public interface ISupervisor
    {
       Task<byte[][]> GetBillsBinary();
    }
}
