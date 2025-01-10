using Project.LevelSystem;
using VContainer.Unity;

namespace Project.Menu
{
    public class MenuEntryPoint : IStartable
    {
        private LevelSelector _levelSelector;
        private MainMenuPanel _mainMenuPanel;
        private JsonLevelParser _jsonLevelParser;

        public MenuEntryPoint(LevelSelector levelSelector, MainMenuPanel mainMenuPanel, JsonLevelParser jsonLevelParser)
        {
            _levelSelector = levelSelector;
            _mainMenuPanel = mainMenuPanel;
            _jsonLevelParser = jsonLevelParser;
        }

        public void Start() { }
    }
}