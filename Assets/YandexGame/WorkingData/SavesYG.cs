using System.Collections.Generic;
using Project.Abilities;
using Project.Entities.Collectables;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Ваши сохранения
        public int unlockedLevelNumber = 1;

        public bool IsSawTraining = false;

        public List<string> UnlockedAbilityNames;
        public List<string> CollectedSnowflakes;

        public bool UnlockAbility(Ability ability)
        {
            string abilityName = ability.GetData().AbilityName;

            if (UnlockedAbilityNames.Contains(abilityName))
                return false;

            UnlockedAbilityNames.Add(abilityName);
            YandexGame.SaveProgress();

            return true;
        }

        public bool UnlockCollectableSnowflake(CollectableSnowflake snowflake)
        {
            string objectName = snowflake.name.Replace("(Clone)", "");

            if (CollectedSnowflakes.Contains(objectName))
                return false;

            CollectedSnowflakes.Add(objectName);
            YandexGame.SaveProgress();

            return true;
        }

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны

        // Вы можете выполнить какие-то действия при загрузке сохранений
        public SavesYG()
        {
            UnlockedAbilityNames = new List<string>();
            CollectedSnowflakes = new List<string>();
        }
    }
}