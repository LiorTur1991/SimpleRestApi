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

        [DataMember(Name ="project_name")]
        public string Name { get; set; }

        [DataMember(Name = "refer_to")]
        public string ReferTo { get; set; }

    }
}