using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Goon.Integracao.Plugins.FromJdEdwards.ToCrm.goon_grupoeconomico 
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
                    // RODO Leandro: REPARTIR EM ARQUIVOS A CADA MILHAO DE CARACTERES (TRANSFORMAR EM PRE CREATE)

                    this.PersistenciaRobo(service, context, goon_ecgmultiline);
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                this.RegistraLogErro(ex, goon_ecgmultiline, service);
            }
            catch (Exception ex)
            {
                this.RegistraLogErro(ex, goon_ecgmultiline, service);
            }
        }

        private void PersistenciaRobo(IOrganizationService service, IPluginExecutionContext context, string content)
        {
            var retContentFormatado = this.ParseContentDw(content);

            foreach (var item in retContentFormatado)
            {
                var newAccount = new Entity("goon_grupoeconomico"); // revisar nome da entidade
                newAccount["goon_code"] = item.Value; // suposicao, efetuar verificacao de nome de campos
                newAccount["goon_description"] = item.Value; //...........
                newAccount["picklistqq"] = item.Value; //.............
                service.Create(newAccount);
            }


        }

        //REVISAR SE O FORMATO SGTRING AO INVES DO LOCAL DE ARQUIVO FUNCIONOU
        private IDictionary<string, string> ParseContentDw(string content)
        {
            var content_ = content.Replace("\"", "").Replace("\n", "*");

            IDictionary<string, string> dicContent = new Dictionary<string, string>();

            var linhas = content.Split('*');
            int cont = 0;

            foreach (var linha in linhas)
            {
                var colunas = linha.Split(';');

                if (cont != 0)
                {
                    dicContent.Add("CODE", colunas[0]);
                    dicContent.Add("DESCRIPTION01", colunas[1]);
                    dicContent.Add("TERCEIRO", colunas[2]);
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