using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Goon.Integracao.Plugins.FromJdEdwards.ToCrm
{
	public class PreCreate : IPlugin
	{
		#region PRE/POS Image Entity

		private readonly string preImageAlias = "preimage";
		private readonly string postImageAlias = "postimage";

		#endregion PRE/POS Image Entity 


		public void Execute(IServiceProvider serviceProvider)
		{

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracing = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            //(context.Depth > 1)
            //    return;

            if (!context.MessageName.Equals("create", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!context.PrimaryEntityName.Equals("goon_ecg", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!context.InputParameters.Contains("Target"))
                return;

            if (!(context.InputParameters["Target"] is Entity))
                return;

            var goon_ecgmultiline = "";

            var entity = context.InputParameters["Target"] as Entity;

            goon_ecgmultiline = entity.GetAttributeValue<string>("goon_ecgmultiline");
            IOrganizationService service =
                   ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(null);

            try
            {

           
                if (entity.Contains("goon_ecgmultiline"))
                {

                    this.PersistenciaRobo(service, context,("goon_ecgmultiline"));
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                this.RegistraLogErro( ex, goon_ecgmultiline, service);
            }
            catch (Exception ex)
            {
                this.RegistraLogErro(ex, goon_ecgmultiline, service);
            }
        }

        private void PersistenciaRobo(IOrganizationService service, IPluginExecutionContext context, string tipoContrato)
        {
            var newConta = new Entity("account");
            newConta["cnpj"] = "99999999999999";
            newConta["picklistqq"] = new OptionSetValue(32322121);
            newConta["picklistqq"] = new EntityReference("contact", new Guid(""));
            service.Create(newConta);
        }

        private void RegistraLogErro(Exception ex, string goon_ecgmultiline, IOrganizationService service)
        {
            var newConta = new Entity("goon_logErros");
            //criar campos na entidade ECG
            //newConta["message"] = goon_messageerror;
            //newConta["body"] = goon_ecgbody;
            //newConta["robo"] = goon_ecgroboname;       // nome da entidade  
            //newConta["status"] = goon_ecgstatus;      //true false
            service.Create(newConta);
        }
    }
	
}