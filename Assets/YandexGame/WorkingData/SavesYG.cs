using System.Collections.Generic;
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

        public void UnlockAbility(CollectionModel collectionModel)
        {
            if (UnlockedAbilityNames.Contains(collectionModel.AbilityName))
                return;

            UnlockedAbilityNames.Add(collectionModel.AbilityName);
        }

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны

        // Вы можете выполнить какие-то действия при загрузке сохранений
        public SavesYG()
        {
            UnlockedAbilityNames = new List<string>();
        }
    }
}