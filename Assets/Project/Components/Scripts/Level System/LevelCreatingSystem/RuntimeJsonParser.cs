using Project.Components.Scripts.Level_System.LevelStructure;

namespace Project.Components.Scripts.Level_System.LevelCreatingSystem
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