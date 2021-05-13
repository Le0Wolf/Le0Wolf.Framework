// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common
{
    /// <summary>
    ///     Интерфейс сущности, содержащей идентификатор
    /// </summary>
    public interface IHaveId
    {
        /// <summary>
        ///     Идентификатор сущности
        /// </summary>
        public long Id { get; set; }
    }
}