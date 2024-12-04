using Project.LevelSystem.LevelStructure;

namespace Project.LevelSystem.LevelCreatingSystem
{
    public class RuntimeJsonParser : JsonLevelParser
    {
        public override LevelDataList GetLevelDataList()
        {
            _levelDataList = null;

            return base.GetLevelDataList();
        }
    }
}