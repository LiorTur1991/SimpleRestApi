using Microsoft.VisualBasic.ApplicationServices;
using SimpleRestApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SimpleRestApi.Controllers
{
    [RoutePrefix("api")]
    public class ProjectsController : ApiController
    {
        private static readonly AutorizationService _autorizationService = AutorizationService.getInstance();
        private static readonly MicrosoftSqlDBService _microsoftSqlDB = MicrosoftSqlDBService.getInstance();

        [Route("Login")]
        [ResponseType(typeof(User))]
        //GET api/test
        public HttpResponseMessage Get(string user, string password)
        {
            HttpResponseMessage Response;
            UserInfo userInfo = _autorizationService.CheckUserToken(user, password);
            if (userInfo == null)
                Response = Request.CreateResponse(HttpStatusCode.Unauthorized, "The username and password you entered did not match our records. " +
                    "Please double-check and try again.");
            else
            {
                Response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
            }
            return Response;
        }

        [Route("GetProjects")]
        //GET api/test
        public HttpResponseMessage Get()
        {
            HttpResponseMessage Response;
            var re = Request;
            var headers = re.Headers;
            string token = null;
            if (headers.Contains("Authorization"))
            {
                token = headers.GetValues("Authorization").First();
            }

            if (_autorizationService.checkIfVerify(token,out var userName)) 
                return _microsoftSqlDB.getUserProjects(userName);

            return new List<ProjectInfo>();
        }



    }
}
