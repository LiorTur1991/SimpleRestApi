using System.Runtime.Serialization;

namespace SimpleRestApi.Models
{
    [DataContract]
    public class UserInfo
    {
        public UserInfo(string name, string password, string token)
        {
            this.FullName = name;
            this.Password = password;
            this.Token = token;
        }
        [DataMember(Name = "name")]
        public string FullName { get; set; }

        [DataMember(Name ="password")]
        public string Password { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }

    }
}