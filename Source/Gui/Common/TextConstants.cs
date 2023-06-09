using System.Collections.Generic;
using System.Linq;

namespace Gui.Common
{
    public static class TextConstants
    {
        public const string DialogIdentifier = "Root";

        public const string ErrorHeader = "Ошибка";
        public const string InfoHeader = "Информация";
        public const string AttenstionHeader = "Внимание";

        public const string PcSelectionError = @"Компьютеры не выбраны.";
        public const string AnalysisSelectionError = @"Проверки не выбраны.";
        public const string BenchmarkSelectionError = @"Шаблон не выбран.";
        public const string SelectionToDeleteError = @"Ни один элемент для удаления не выбран.";

        public const string HomeModelAnalyzeSuccess = @"Анализ успешно выполнен.";
        public const string ItemAddSuccess = @"Элемент успешно добавлен.";
        public const string ItemUpdateSuccess = @"Элемент успешно обновлён.";
        public const string ItemDeleteSuccess = @"Элемент успешно удалён.";
        public const string ItemsDeleteSuccess = @"Элементы успешно удалены.";

        public const string BenchmarksModelAddSuccess = @"Шаблон успешно добавлен.";
        public const string BenchmarksModelUpdateSuccess = @"Шаблон успешно обновлён.";
        public const string BenchmarksModelCopySuccess = @"Шаблон успешно скопирован.";
        public const string BenchmarksModelAttachSuccess = @"Шаблон успешно прикреплён.";
        public const string BenchmarksModelDetachSuccess = @"Шаблон успешно откреплён.";
        public const string BenchmarksModelRemoveOneSuccess = @"Шаблон успешно удалён.";
        public const string BenchmarksModelRemoveManySuccess = @"Шаблоны успешно удалены.";

        public const string SchedulerModelAddSuccess = @"Запись планировщика успешно добавлена.";
        public const string SchedulerModelUpdateSuccess = @"Запись планировщика успешно обновлена.";
        public const string SchedulerModelRemoveOneSuccess = @"Запись планировщика успешно удалена.";
        public const string SchedulerModelRemoveManySuccess = @"Записи планировщика успешно удалены.";

        private const string ContactSupport = @"Обратитесь в поддержку.";

        public const string UnidentifiedErrorOccured = @$"Ошибка. {ContactSupport}"; // maybe Непредвиденная/неопознанная
        public const string ConnectionErrorOccured = @$"Не удаётся подключиться к серверу. Приложение будет закрыто. {ContactSupport}";

        public const string GetHostsError = @$"Не удалось получить компьютеры в сети. {ContactSupport}";
        public const string HomeModelAnalyzeError = @$"Не удалось выполнить анализ. {ContactSupport}";

        public const string AnalysisResultsModelGetError = @$"Не удалось получить результаты проверок. {ContactSupport}";

        public const string BenchmarksModelGetError = @$"Не удалось получить шаблоны. {ContactSupport}";
        public const string BenchmarkModelGetItemsError = @$"Не удалось получить элементы шаблона. {ContactSupport}";
        public const string BenchmarksModelAddError = @$"Не удалось добавить шаблон. {ContactSupport}";
        public const string BenchmarksModelUpdateError = @$"Не удалось обновить шаблон. {ContactSupport}";
        public const string BenchmarksModelRemoveError = @$"Не удалось удалить шаблон. {ContactSupport}";
        public const string BenchmarksModelDetachError = @$"Не удалось открепить шаблон. {ContactSupport}";
        public const string BenchmarksModelAttachError = @$"Не удалось прикрепить шаблон. {ContactSupport}";
        public const string BenchmarksModelCopyError = @$"Не удалось скопировать шаблон. {ContactSupport}";
        public const string BenchmarksModelGetFromFileError = @$"Не удалось получить шаблон из CSV-файла. {ContactSupport}";

        public const string SchedulerModelGetError = @$"Не удалось получить записи планировщика. {ContactSupport}";
        public const string SchedulerModelAddOrUpdateError = @$"Не удалось добавить или обновить запись планировщика. {ContactSupport}";
        public const string SchedulerModelRunError = @$"Не удалось запустить запись планировщика. {ContactSupport}";

        public static string GetReallyWantDelete<TModel>(List<TModel> itemsToDelete)
        {
            return @$"Вы действительно хотите удалить выбранные элементы: {itemsToDelete.Count} шт?";
        }
    }
}
