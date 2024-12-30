using Project.LevelSystem.LevelStructure;

namespace Project.LevelSystem.LevelCreatingSystem
{
    public class RuntimeJsonParser : JsonLevelParser
    {
        public override LevelDataList GetLevelDataList()
        {
            LevelDataList = null;

            return base.GetLevelDataList();
        }
    }
}