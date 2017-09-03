using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Data.Models
{

    public class LoginResponseModel : BaseResponseModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
        public long CookId { get; set; }
        public string CookFullName { get; set; }
    }


    public class PostRegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string PhotoUrl { get; set; }

        [JsonIgnore]
        public PhotoModel Photo { get; set; }

        [JsonIgnore]
        public string RemovedPhotoUrl { get; set; }
    }

    public class UserInfoModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }

        public long CookId { get; set; }

        public string CookFullName { get; set; }
    }

}
