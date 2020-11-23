using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Goon.Integracao.Plugins.FromJdEdwards.ToCrm.account
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

            (context.Depth > 1)
                return;

            if (!context.MessageName.Equals("create", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!context.PrimaryEntityName.Equals("goon_crmadb", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!context.InputParameters.Contains("Target"))
                return;

            if (!(context.InputParameters["Target"] is Entity))
                return;

            var goon_adbmultiline = "";

            var entity = context.InputParameters["Target"] as Entity;

            goon_ecgmultiline = entity.GetAttributeValue<string>("goon_adbmultiline");
            IOrganizationService service =
                   ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(null);

            try
            {

           
                if (entity.Contains("goon_adbmultiline"))
                {
                    // RODO Leandro: REPARTIR EM ARQUIVOS A CADA MILHAO DE CARACTERES (TRANSFORMAR EM PRE CREATE)
                    
                    this.PersistenciaRobo(service, context, goon_adbmultiline);
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                this.RegistraLogErro( ex, goon_adbmultiline, service);
            }
            catch (Exception ex)
            {
                this.RegistraLogErro(ex, goon_adbmultiline, service);
            }
        }

        private void PersistenciaRobo(IOrganizationService service, IPluginExecutionContext context, string content)
        {
            var retContentFormatado = this.ParseContentDw(content);

            foreach (var item in retContentFormatado)
            {
                var newAccount = new Entity("Account"); // revisar nome da entidade
                newAccount["goon_code"] = item.Value; // suposicao, efetuar verificacao de nome de campos
                newAccount["goon_description"] = item.Value; //...........
                newAccount["picklistqq"] = item.Value; //.............
                service.Create(newAccount);
            }

            
        }

        //REVISAR SE O FORMATO SGTRING AO INVES DO LOCAL DE ARQUIVO FUNCIONOU
        private IDictionary<string, string> ParseContentDw (string content)
        {
            var content_ = content.Replace("\"", "").Replace("\n", "*");

        IDictionary<string, string> dicContent = new Dictionary<string, string>();

            var linhas = content.Split('*') ;
            int cont = 0;

                foreach (var linha in linhas)
                {
                    var colunas = linha.Split(';');

                    if (cont != 0) {
                    dicContent.Add("JDE NUMBER", colunas[0]);
                    dicContent.Add("CNPJ", colunas[1]);
                    dicContent.Add("CUSTOMER NAME", colunas[2]);
                    dicContent.Add("Business Unit", colunas[3]);
                    dicContent.Add("TYPE A/B", colunas[4]);
                    dicContent.Add("TYPE CUST", colunas[5]);
                    dicContent.Add("SALESPERSON CODE", colunas[6]);
                    dicContent.Add("SALESPERSON NAME", colunas[7]);
                    dicContent.Add("GEOG REG", colunas[8]);
                    dicContent.Add("GEOG REG DESCR", colunas[9]);
                    dicContent.Add("CUSTOMER GROUP", colunas[10]);
                    dicContent.Add("CUSTOMER GROUP DESC", colunas[11]);
                    dicContent.Add("ADDRESS - STREET", colunas[12]);
                    dicContent.Add("ADDRESS - COMPLEMENTAL", colunas[13]);
                    dicContent.Add("ADDRESS - POSTAL CODE", colunas[14]);
                    dicContent.Add("ADDRESS - CITY", colunas[15]);
                    dicContent.Add("ADDRESS - DISTRICT", colunas[16]);
                    dicContent.Add("ADDRESS - STATE", colunas[17]);
                    dicContent.Add("ADDRESS - COUNTRY", colunas[18]);
              
                }

                    cont++;
                }

            return dicContent;

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