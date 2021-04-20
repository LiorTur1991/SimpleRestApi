﻿using Microsoft.VisualBasic.ApplicationServices;
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

        [Route("authenticate")]
        [ResponseType(typeof(User))]
        //GET api/test
        public HttpResponseMessage Post(string user, string password)
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

        [Route("info")]
        //GET api/test
        public HttpResponseMessage Get()
        {
            HttpResponseMessage Response;
            List<ProjectInfo> result = new List<ProjectInfo>();
            var request = Request;
            var headers = request.Headers;
            string token = null;
            if (headers.Contains("Authorization"))
            {
                token = headers.GetValues("Authorization").First();
            }
            else
                Response = Request.CreateResponse(HttpStatusCode.Unauthorized, "Verify authorization header exist");

            if (_autorizationService.checkIfVerify(token, out var userName))
            {
                result = _microsoftSqlDB.getUserProjects(userName);
                if (!result.Any())
                    Response = Request.CreateResponse(HttpStatusCode.OK, "No projects refer to this user");
                else 
                    Response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else {
                Response = Request.CreateResponse(HttpStatusCode.Unauthorized, "Autorization Failed, Please log in first");
            }
            return Response;
        }
    }
}
