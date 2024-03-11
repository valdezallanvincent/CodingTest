using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Core.Models
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public ErrorResponse Error { get; private set; }

        public static ServiceResult<T> Success(T data) => new ServiceResult<T> { IsSuccess = true, Data = data };
        public static ServiceResult<T> Failure(ErrorResponse error) => new ServiceResult<T> { IsSuccess = false, Error = error };
    }
}
