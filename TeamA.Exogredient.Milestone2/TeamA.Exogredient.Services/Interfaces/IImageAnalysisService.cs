using System.Collections.Generic;
using TeamA.Exogredient.DataHelpers;
using Google.Cloud.Vision.V1;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TeamA.Exogredient.Services.Interfaces
{
    public interface IImageAnalysisService
    {
        Task<AnalysisResult> AnalyzeAsync(Image image, ICollection<string> categories);
    }
}
