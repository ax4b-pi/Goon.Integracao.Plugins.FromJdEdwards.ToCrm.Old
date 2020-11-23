using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Goon.Integracao.Plugins.FromJdEdwards.ToCrm.goon_nfe
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

            if (!context.PrimaryEntityName.Equals("goon_crmnfe", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!context.InputParameters.Contains("Target"))
                return;

            if (!(context.InputParameters["Target"] is Entity))
                return;

            var goon_nfemultiline = "";

            var entity = context.InputParameters["Target"] as Entity;

            goon_nfemultiline = entity.GetAttributeValue<string>("goon_nfemultiline");
            IOrganizationService service =
                   ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(null);

            try
            {

           
                if (entity.Contains("goon_nfemultiline"))
                {
                    // RODO Leandro: REPARTIR EM ARQUIVOS A CADA MILHAO DE CARACTERES (TRANSFORMAR EM PRE CREATE)
                    
                    this.PersistenciaRobo(service, context, goon_nfemultiline);
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                this.RegistraLogErro( ex, goon_nfemultiline, service);
            }
            catch (Exception ex)
            {
                this.RegistraLogErro(ex, goon_nfemultiline, service);
            }
        }

        private void PersistenciaRobo(IOrganizationService service, IPluginExecutionContext context, string content)
        {
            var retContentFormatado = this.ParseContentDw(content);

            foreach (var item in retContentFormatado)
            {
                var newAccount = new Entity("goon_crmnfe"); // revisar nome da entidade
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
                    dicContent.Add("SPECIES", colunas[0]);
                    dicContent.Add("SPECIES DESCRIPTION", colunas[1]);
                    dicContent.Add("INVOICE NUMBER", colunas[2]);
                    dicContent.Add("INVOICE SERIAL", colunas[3]);
                    dicContent.Add("JDE CODE INVOICE", colunas[4]);
                    dicContent.Add("JDE LINE INVOICE", colunas[5]);
                    dicContent.Add("ORDER NUMBER", colunas[6]);
                    dicContent.Add("ORDER TYPE", colunas[7]);
                    dicContent.Add("LINE TYPE", colunas[8]);
                    dicContent.Add("DOC NUMBER", colunas[9]);
                    dicContent.Add("COMPANY", colunas[10]);
                    dicContent.Add("ITEM", colunas[11]);
                    dicContent.Add("BUSINESS UNIT", colunas[12]);
                    dicContent.Add("LOT NUMBER", colunas[13]);
                    dicContent.Add("ITEM DESCRIPTION", colunas[14]);
                    dicContent.Add("UOM", colunas[15]);
                    dicContent.Add("QUANTITY", colunas[16]);
                    dicContent.Add("UNIT PRICE", colunas[17]);
                    dicContent.Add("TOTAL PRICE", colunas[18]);
                    dicContent.Add("INVOICE DISCOUNT", colunas[19]);
                    dicContent.Add("JDE CUSTOMER NUMBER", colunas[20]);
                    dicContent.Add("TAX ICMS", colunas[21]);
                    dicContent.Add("TAX IPI", colunas[22]);
                    dicContent.Add("% TAX IPI", colunas[23]);
                    dicContent.Add("TAX PIS", colunas[24]);
                    dicContent.Add("% TAX PIS", colunas[25]);
                    dicContent.Add("TAX COFINS", colunas[26]);
                    dicContent.Add("% TAX COFINS", colunas[27]);
                    dicContent.Add("ORDER ISSUE DATE", colunas[28]);
                    dicContent.Add("ORDER STATUS LAST", colunas[29]);
                    dicContent.Add("ORDER STATUS NEXT", colunas[30]);
                    dicContent.Add("CURRENCY", colunas[31]);
                    dicContent.Add("EXCHANGE RATE", colunas[32]);
                    dicContent.Add("PAYMENT INSTRUCTION", colunas[33]);
                    dicContent.Add("PAYMENT", colunas[34]);
                    dicContent.Add("SPLIT DATE BEGIN", colunas[35]);
                    dicContent.Add("SPLIT DATE END", colunas[36]);
                    dicContent.Add("% SPLIT", colunas[37]);
                    dicContent.Add("SPLIT SALEPERSON", colunas[38]);
                    dicContent.Add("QTY 1", colunas[39]);
                    dicContent.Add("UOM 1", colunas[40]);
                    dicContent.Add("QTY 2", colunas[41]);
                    dicContent.Add("UOM 2", colunas[42]);
                    dicContent.Add("THEOR", colunas[43]);
                    dicContent.Add("QTY 3", colunas[44]);
                    dicContent.Add("UOM 3", colunas[45]);
                    dicContent.Add("SPLIT TOT PRICE", colunas[46]);
                    dicContent.Add("SPLIT ICMS", colunas[47]);
                    dicContent.Add("SPLI DISCOUNT", colunas[48]);
                    dicContent.Add("SPLIT IPI", colunas[49]);
                    dicContent.Add("SPLIT PIS", colunas[50]);
                    dicContent.Add("SPLIT COFINS", colunas[51]);
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