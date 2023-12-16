using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace GameKit
{
    public interface IEntityJsonData : IEntityData
    {
        string sectionId { get; }

        [NotNull]
        JObject ToJson();

        void LoadData([NotNull] JObject json);
    }
}