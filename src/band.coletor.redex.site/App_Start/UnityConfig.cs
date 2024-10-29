using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Infra.Repositorios;
using System;

using Unity;

namespace Band.Coletor.Redex.Site
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
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

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;

        #endregion Unity Container

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IConteinerRepositorio, ConteinerRepositorio>();
            container.RegisterType<ITalieRepositorio, TalieRepositorio>();
            container.RegisterType<IUsuarioLoginRepositorio, UsuarioLoginRepositorio>();
            container.RegisterType<ILacreRepositorio, LacreRespositorio>();
            container.RegisterType<IRegistroRepositorio, RegistroRepositorio>();
            container.RegisterType<IReservaRepositorio, ReservaRepositorio>();
            container.RegisterType<IPreRegistroRepositorio, PreRegistroRepositorio>();
            container.RegisterType<IConferenteRepositorio, ConferenteRepositorio>();
            container.RegisterType<IEquipeRepositorio, EquipeRepositorio>();
            container.RegisterType<IOperacaoRepositorio, OperacaoRepositorio>();
            container.RegisterType<IEstufagemRepositorio, EstufagemRepositorio>();
            container.RegisterType<ISaidaCaminhaoRepositorio,SaidaCaminhaoRepositorio>();
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








        }
    }
}