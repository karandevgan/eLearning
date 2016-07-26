﻿using Learning.Data;
using Ninject;
using System;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Threading;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Principal;
using System.Net.Http;
using System.Net;

namespace Learning.Web.Filters {
    public class LearningAuthorizeAttribute : AuthorizationFilterAttribute
    {
        [Inject]
        public LearningRepository TheRepository { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // If user is authenticated using form authentication
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return;
            }

            var authHeader = actionContext.Request.Headers.Authorization;
            if (authHeader != null)
            {
                if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && !String.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var credArray = GetCredentials(authHeader);
                    var userName = credArray[0];
                    var password = credArray[1];

                    if (IsResourceOwner(userName, actionContext))
                    {
                        if (TheRepository.LoginStudent(userName, password))
                        {
                            var currentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                            Thread.CurrentPrincipal = currentPrincipal;
                            return;
                        }
                    }
                }
            }
            HandleUnauthorizedRequest(actionContext);
        }

        private void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate",
                "Basic Scheme='eLearning' location='http://localhost:54026/account/login'");
        }

        private bool IsResourceOwner(string userName, HttpActionContext actionContext)
        {
            var routeData = actionContext.Request.GetRouteData();
            var resourceUserName = routeData.Values["userName"] as string;

            return (resourceUserName == userName);
        }

        private string[] GetCredentials(AuthenticationHeaderValue authHeader)
        {
            var rawCred = authHeader.Parameter;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var cred = encoding.GetString(Convert.FromBase64String(rawCred));
            var credArray = cred.Split(':');

            return credArray;
        }
    }
}