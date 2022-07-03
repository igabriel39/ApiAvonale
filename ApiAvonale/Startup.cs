using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace ApiAvonale
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Routes.MapHttpRoute(
            name: "RotaDinamica",
            routeTemplate: "{folder}/{controller}",
            defaults: new
            {
                token = RouteParameter.Optional,
            }

           );
            app.UseWebApi(config);
        }
    }
}
