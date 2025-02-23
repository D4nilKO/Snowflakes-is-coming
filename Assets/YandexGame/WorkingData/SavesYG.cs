using System.Collections.Generic;
using Project.Abilities;
using Project.Data.Collections;

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

        public void UnlockAbility(Ability ability)
        {
            if (UnlockedAbilityNames.Contains(ability.GetData().AbilityName))
                return;

            UnlockedAbilityNames.Add(ability.GetData().AbilityName);
        }

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны

        // Вы можете выполнить какие-то действия при загрузке сохранений
        public SavesYG()
        {
            UnlockedAbilityNames = new List<string>();
        }
    }
}