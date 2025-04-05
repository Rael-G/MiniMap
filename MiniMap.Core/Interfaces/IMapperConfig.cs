namespace MiniMap
{
    public interface IMapperConfig
    {
        /// <summary>
        /// Método chamado durante a configuração do mapeador. 
        /// Utilize-o para registrar mapeamentos personalizados no <see cref="MapperService"/>.
        /// </summary>
        /// <param name="mapperService">Instância do serviço de mapeamento que receberá as configurações.</param>
        void Configure(MapperService mapperService);
    }
}
