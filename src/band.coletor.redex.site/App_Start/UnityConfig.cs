using Band.Coletor.Redex.Business;
using Band.Coletor.Redex.Business.Classes;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Infra.Repositorios;
using System;
using AutoMapper;
using Unity;
using Band.Coletor.Redex.Business.Mapping;
using System.Configuration;
using Unity.Injection;

namespace Band.Coletor.Redex.Site
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        public static IUnityContainer Container => container.Value;
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            // Obter a connectionString do web.config
            string connectionString = ConfigurationManager.ConnectionStrings["StringConexaoSqlServer"].ConnectionString;

            // Repositórios
            //container.RegisterType<IConteinerRepositorio, ConteinerRepositorio>();
            //container.RegisterType<ITalieRepositorio, TalieRepositorio>();
            container.RegisterType<IUsuarioLoginRepositorio, UsuarioLoginRepositorio>();
            container.RegisterType<ILacreRepositorio, LacreRespositorio>();
            //container.RegisterType<IRegistroRepositorio, RegistroRepositorio>();
            container.RegisterType<IReservaRepositorio, ReservaRepositorio>();
            container.RegisterType<IPreRegistroRepositorio, PreRegistroRepositorio>();
            //container.RegisterType<IConferenteRepositorio, ConferenteRepositorio>();
            //container.RegisterType<IEquipeRepositorio, EquipeRepositorio>();
            //container.RegisterType<IOperacaoRepositorio, OperacaoRepositorio>();
            container.RegisterType<IEstufagemRepositorio, EstufagemRepositorio>();
            container.RegisterType<ISaidaCaminhaoRepositorio, SaidaCaminhaoRepositorio>();
            container.RegisterType<IIventarioCNTRRepositorio, IventarioCNTRRepositorio>();
            container.RegisterType<IIventarioCSRepositorio, IventarioCSRepositorio>();
            container.RegisterType<IPatiosRepositorio, PatiosRepositorio>();
            container.RegisterType<ITalieColetorDescargaRepositorio, TalieColetorDescargaRepositorio>();
            container.RegisterType<IMovimentacaoCargaSoltaRepositorio, MovimentacaoCargaSoltaRepositorio>();
            container.RegisterType<IMovimentacaoContainerRepositorio, MovimentacaoContainerRepositorio>();
            container.RegisterType<IArmazensRepositorio, ArmazensRepositorio>();
            container.RegisterType<IMotivosRepositorio, MotivosRepositorio>();
            container.RegisterType<IIventarioCegoRepositorio, IventarioCegoRepositorio>();
            container.RegisterType<IVeiculosRepositorio, VeiculosRepositorio>();
            container.RegisterType<IAutorizaSaidaRepository, AutorizaSaidaRepository>();
            container.RegisterType<IDescargaExportacaoRepositorio, DescargaExportacaoRepositorio>();

           

            // Registrar repositórios com a connectionString injetada
            container.RegisterType<IEquipeRepositorio, EquipeRepositorio>(new InjectionConstructor(connectionString));
            container.RegisterType<IConferenteRepositorio, ConferenteRepositorio>(new InjectionConstructor(connectionString));
            container.RegisterType<IArmazenRepositorio, ArmazemRepositorio>(new InjectionConstructor(connectionString));
            container.RegisterType<IOperacaoRepositorio, OperacaoRepositorio>(new InjectionConstructor(connectionString));
            container.RegisterType<ITalieRepositorio, TalieRepositorio>(new InjectionConstructor(connectionString));
            container.RegisterType<IConteinerRepositorio, ConteinerRepositorio>(new InjectionConstructor(connectionString));
            container.RegisterType<IRegistroRepositorio, RegistroRepositorio>(new InjectionConstructor(connectionString));


            // Camada de Negócios (Business)
            container.RegisterType<IDescargaExportacaoBusiness, DescargaExportacaoBusiness>();
            container.RegisterType<IEquipeBusiness, EquipeBusiness>();
            container.RegisterType<IConferenteBusiness, ConferenteBusiness>();
            container.RegisterType<IOperacaoBusiness, OperacaoBusiness>();
            container.RegisterType<IArmazemBusiness, ArmazenBusiness>();
            container.RegisterType<ITalieBusiness, TalieBusiness>();
            container.RegisterType<IConteinerBusiness, ContainerBusiness>();
            container.RegisterType<IRegistroBusiness, RegistroBusiness>();

            // Configuração do AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();
            container.RegisterInstance(mapper); // Registra o IMapper como singleton no contêiner do Unity
        }
    }

}