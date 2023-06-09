using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Gui.Views;

namespace Gui.Common
{
    public static class AnalysisProvider
    {
        // private static readonly Dictionary<string, Page> PageByNames = new ()
        // {
        //     { @"Анализ конфигураций", new ResultConfigurationDetailPage(null)},
        //     { @"Анализ устройств", new ResultSoftwareDetailPage(null) },
        //     { @"Анализ ПО", new ResultHardwareDetailPage(null) },
        //     { @"Анализ обновлений ПО", new ResultUpgradableSoftwarePage(null) },
        // };

        private static readonly string[] analysisNames = {@"Анализ конфигураций", @"Анализ устройств", @"Анализ ПО", @"Анализ обновлений ПО"};

        // public static Page GetPageByName(string analyzerName)
        // {
        //     if (!PageByNames.TryGetValue(analyzerName, out var page))
        //     {
        //         //return new Page(@$"Проверки {analyzerName} не существует.");
        //     }
        //
        //     return page;
        // }

        public static List<string> GetNamesBySelectedAnalysisModel(SelectedAnalysisModel model)
        {
            var selectedAnalysisNames = new List<string>();
            if (model.IsConfigurationSelected)
            {
                selectedAnalysisNames.Add(analysisNames[0]);
            }

            if (model.IsHardwareSelected)
            {
                selectedAnalysisNames.Add(analysisNames[1]);
            }

            if (model.IsSoftwareSelected)
            {
                selectedAnalysisNames.Add(analysisNames[2]);
            }

            if (model.IsSoftwareUpgradeSelected)
            {
                selectedAnalysisNames.Add(analysisNames[3]);
            }

            return selectedAnalysisNames;
        }

        public static SelectedAnalysisModel GetSelectedAnalysisModelByNames(List<string> selectedAnalysisNames)
        {
            var model = new SelectedAnalysisModel();
            if (selectedAnalysisNames.Contains(analysisNames[0]))
            {
                model.IsConfigurationSelected = true;
            }

            if (selectedAnalysisNames.Contains(analysisNames[1]))
            {
                model.IsHardwareSelected = true;
            }

            if (selectedAnalysisNames.Contains(analysisNames[2]))
            {
                model.IsSoftwareSelected = true;
            }

            if (selectedAnalysisNames.Contains(analysisNames[3]))
            {
                model.IsSoftwareUpgradeSelected = true;
            }

            return model;
        }
    }
}
