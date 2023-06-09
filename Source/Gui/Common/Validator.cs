using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gui.Common
{
    internal static class Validator
    {
        public static string ValidateString(string str, string name)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return $"Поле {name} не заполнено\n";
            }

            return string.Empty;
        }

        public static string ValidateStrings(List<Tuple<string, string>> input)
        {
            var errorString = new StringBuilder();
            if (!input.Any())
            {
                return string.Empty;
            }

            foreach (var item in input)
            {
                if (string.IsNullOrWhiteSpace(item.Item1))
                {
                    errorString.AppendLine($"Поле {item.Item2} не заполнено\n");
                }
            }

            return errorString.ToString();
        }

        public static string ValidateRecurence(string inputDays, string inputHours, string inputMinutes)
        {
            if (string.IsNullOrWhiteSpace(inputDays) &&
                string.IsNullOrWhiteSpace(inputHours) &&
                string.IsNullOrWhiteSpace(inputMinutes))
            {
                return "Повторения не заданы";
            }

            var errorString = new StringBuilder();
            var isAllNull = true;
            if (!string.IsNullOrWhiteSpace(inputDays))
            {
                if (!int.TryParse(inputDays, out var days))
                {
                    errorString.AppendLine("В поле для дней введено не число");
                }

                if (days < 0)
                {
                    errorString.AppendLine("В поле дней введено значение меньше 0");
                }

                if (days > 30)
                {
                    errorString.AppendLine("В поле дней введено значение больше 30");
                }

                if (days != 0)
                {
                    isAllNull = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(inputHours))
            {
                if (!int.TryParse(inputHours, out var hours))
                {
                    errorString.AppendLine("В поле для часов введено не число");
                }

                if (hours < 0)
                {
                    errorString.AppendLine("В поле часов введено значение меньше 0");
                }

                if (hours > 24)
                {
                    errorString.AppendLine("В поле часов введено значение больше 24");
                }

                if (hours != 0)
                {
                    isAllNull = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(inputMinutes))
            {
                if (!int.TryParse(inputMinutes, out int minuts))
                {
                    errorString.AppendLine("В поле для минут введено не число");
                }

                if (minuts < 0)
                {
                    errorString.AppendLine("В поле минут введено значение меньше 0");
                }

                if (minuts > 60)
                {
                    errorString.AppendLine("В поле минут введено значение больше 60");
                }

                if (minuts != 0)
                {
                    isAllNull = false;
                }
            }

            if (errorString.Length == 0 && isAllNull)
            {
                return "Повторения не заданы";
            }

            return errorString.ToString();
        }
    }
}
