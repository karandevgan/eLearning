﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Learning.Web.Filters {
    public class ForceHttpsAttribute: AuthorizationFilterAttribute {
        public override void OnAuthorization(HttpActionContext actionContext) {
            var request = actionContext.Request;

            if (request.RequestUri.Scheme != Uri.UriSchemeHttps) {
                var html = "<p>Https is required</p>";

                if (request.Method.Method == "GET") {
                    actionContext.Response = request.CreateResponse(HttpStatusCode.Found);
                    actionContext.Response.Content = new StringContent(html, Encoding.UTF8, "text/html");

                    UriBuilder httpsNewUri = new UriBuilder(request.RequestUri);
                    httpsNewUri.Scheme = Uri.UriSchemeHttps;
                    // Port for the development
                    httpsNewUri.Port = 44324;
                   
                    //httpsNewUri.Port = 443;

                    actionContext.Response.Headers.Location = httpsNewUri.Uri;
                }
                else {
                    actionContext.Response = request.CreateResponse(HttpStatusCode.NotFound);
                    actionContext.Response.Content = new StringContent(html, Encoding.UTF8, "text/html");
                }
            }
        }
    }
}