using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Data.Models
{
    public abstract class BaseResponseModel
    {
        public bool IsSuccess { get; set; }
        public ERROR_TYPES ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum ERROR_TYPES { BAD_REQUEST, INTERNAL_SERVER_ERROR};
}
