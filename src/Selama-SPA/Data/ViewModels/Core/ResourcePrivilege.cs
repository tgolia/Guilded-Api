using Newtonsoft.Json;
using DataModel = Selama_SPA.Data.Models.Core.ResourcePrivilege;

namespace Selama_SPA.Data.ViewModels.Core
{
    public class ResourcePrivilege
    {
        #region Properties
        #region Public Properties
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        #endregion
        #endregion

        public ResourcePrivilege()
        {
        }

        public ResourcePrivilege(DataModel privilege)
        {
            Id = privilege.Id;
            Name = privilege.Name;
        }
    }
}