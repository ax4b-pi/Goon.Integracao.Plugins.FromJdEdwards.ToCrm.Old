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

			//	IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
			//	ITracingService tracing = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

			//	//if (context.Depth > 1)
			//	//    return;

			//	if (!context.MessageName.Equals("create", StringComparison.InvariantCultureIgnoreCase))
			//		return;

			//	if (!context.PrimaryEntityName.Equals("d365_contratodevendas", StringComparison.InvariantCultureIgnoreCase))
			//		return;

			//	if (!context.InputParameters.Contains("Target"))
			//		return;

			//	if (!(context.InputParameters["Target"] is Entity))
			//		return;

			//	try
			//	{

			//		IOrganizationService systemUserService =
			//			((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(null);

			//		Entity entity = context.InputParameters["Target"] as Entity;
			//		var goon_nomedocampoquetemlinguica = "";

			//		if (!entity.Contains("goon_nomedocampoquetemlinguica"))
			//		{
			//			goon_nomedocampoquetemlinguica = entity.GetAttributeValue<bool>("goon_nomedocampoquetemlinguica")
			//			this.PersistenciaRobo(systemUserService, context, entity.GetAttributeValue<bool>("d365_tipo_contrato_sistec"));
			//		}
			//	}
			//	catch (InvalidPluginExecutionException ex)
			//	{
			//		this.RegistraLogErro(entity, ex, goon_nomedocampoquetemlinguica);
			//	}
			//	catch (Exception ex)
			//	{
			//		this.RegistraLogErro(entity, ex, goon_nomedocampoquetemlinguica);
			//	}
			//}

			//private void PersistenciaRobo(IOrganizationService systemUserService, IPluginExecutionContext context, bool tipoContrato)
			//{
			//	int retornoSeq = 0;
			//	string partRet = "";
			//	string retFormatado = "";

			//	var query = new QueryExpression("d365_contratodevendas");
			//	query.NoLock = true;
			//	query.ColumnSet = new ColumnSet("d365_codigocontratocrm", "createdon", "d365_aditivoversao");
			//	query.Criteria.AddCondition("d365_aditivoversao", ConditionOperator.Equal, 0);
			//	query.Criteria.AddCondition("d365_codigocontratocrm", ConditionOperator.BeginsWith, "SVD");
			//	query.TopCount = 1;
			//	query.AddOrder("createdon", OrderType.Descending);

			//	var entityCollection = systemUserService.RetrieveMultiple(query);
			//	this.RegistraLogErro();

			//		var newConta = new Entity("account");
			//		newConta["cnpj"] = "99999999999999";
			//		newConta["picklistqq"] = new OptionSetValue(32322121);
			//		newConta["picklistqq"] = new EntityReference("contact", new Guid(""));
			//		systemUserService.Create(newConta);
			//}

			//private void RegistraLogErro(Entity entity, Exception ex, goon_nomedocampoquetemlinguica)
			//      {
			//	var newConta = new Entity("goon_logErros");
			//	newConta["message"] = ex.Message;
			//	newConta["body"] = goon_nomedocampoquetemlinguica;
			//	newConta["robo"] = nomeDoRobo;
			//	systemUserService.Create(newConta);
			//}
		}
	}