using System;
using System.Threading.Tasks;

namespace AppContingenciaPex.Service
{
    public interface IPlatformInfo
    {
        object AndroidContext { get; set; }
        object AndroidResource { get; set; }
        string GetAbsolutePath();   // For Android Only
        object GetImgResource();    // Image Resource Type return
        Task<object> GetImgResourceAsync();
    }
}
