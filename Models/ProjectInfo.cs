using System.Runtime.Serialization;

namespace SimpleRestApi.Models
{
    [DataContract]
    public class ProjectInfo
    {
        public ProjectInfo(int id, string projectName, string referTo)
        {
            this.ID = id;
            this.Name = projectName;
            this.ReferTo = referTo;
        }

        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name ="name")]
        public string Name { get; set; }

        [DataMember(Name = "referTo")]
        public string ReferTo { get; set; }

    }
}