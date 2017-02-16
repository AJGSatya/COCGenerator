using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using COCGenerator.BusinesObjects;

namespace COCGenerator
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            bizAssociation biz = new bizAssociation();
            context.Response.ContentType = "image/jpeg";
            byte[] logo = biz.GetAssociationLogo(Convert.ToInt32(context.Request.QueryString["aid"]));
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.OutputStream.Write(logo,0,logo.GetLength(0));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
