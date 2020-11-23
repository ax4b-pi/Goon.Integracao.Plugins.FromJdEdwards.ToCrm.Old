using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Goon.Integracao.Plugins.FromJdEdwards.ToCrm.goon_gra
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

            if (!context.PrimaryEntityName.Equals("goon_crmgra", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!context.InputParameters.Contains("Target"))
                return;

            if (!(context.InputParameters["Target"] is Entity))
                return;

            var goon_gramultiline = "";

            var entity = context.InputParameters["Target"] as Entity;

            goon_gramultiline = entity.GetAttributeValue<string>("goon_gramultiline");
            IOrganizationService service =
                   ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(null);

            try
            {

           
                if (entity.Contains("goon_gramultiline"))
                {
                    // RODO Leandro: REPARTIR EM ARQUIVOS A CADA MILHAO DE CARACTERES (TRANSFORMAR EM PRE CREATE)
                    
                    this.PersistenciaRobo(service, context, goon_gramultiline);
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                this.RegistraLogErro( ex, goon_gramultiline, service);
            }
            catch (Exception ex)
            {
                this.RegistraLogErro(ex, goon_gramultiline, service);
            }
        }

        private void PersistenciaRobo(IOrganizationService service, IPluginExecutionContext context, string content)
        {
            var retContentFormatado = this.ParseContentDw(content);

            foreach (var item in retContentFormatado)
            {
                var newAccount = new Entity("goon_crmgra"); // revisar nome da entidade
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
                    dicContent.Add("FISCALYEAR", colunas[0]);
                    dicContent.Add("CUSTOMER", colunas[1]);
                    dicContent.Add("SHIPTONAME", colunas[2]);
                    dicContent.Add("SALESPERSON", colunas[3]);
                    dicContent.Add("SALES_REP_NAME", colunas[4]);
                    dicContent.Add("SCENARIO", colunas[5]);
                    dicContent.Add("ITEMNUMBER02", colunas[6]);
                    dicContent.Add("DESCRIPTION01", colunas[7]);
                    dicContent.Add("MEASURENAME", colunas[8]);
                    dicContent.Add("JUL", colunas[9]);
                    dicContent.Add("AUG", colunas[10]);
                    dicContent.Add("SEP", colunas[11]);
                    dicContent.Add("OCT", colunas[12]);
                    dicContent.Add("NOV", colunas[13]);
                    dicContent.Add("DEC", colunas[14]);
                    dicContent.Add("JAN", colunas[15]);
                    dicContent.Add("FEB", colunas[16]);
                    dicContent.Add("MAR", colunas[17]);
                    dicContent.Add("APR", colunas[18]);
                    dicContent.Add("MAY", colunas[19]);
                    dicContent.Add("JUN", colunas[20]);
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