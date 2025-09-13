using BulgarianMountainTrails.Core.DTOs;

namespace BulgarianMountainTrails.Core.Helpers
{
    public class ApiException : Exception
    {
        public List<ApiError> Errors { get; }

        public ApiException(List<ApiError> errors)
            : base("One or more errors occurred!")
        {
            Errors = errors;
        }
    }
}
