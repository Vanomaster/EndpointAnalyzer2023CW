using CleanModels.Schedule;

namespace Common.Extensions;

public static class CronExtensions
{
    public static string ToCronExpression(this RecurrenceModel recurrenceModel)
    {
        if (recurrenceModel.StartDateTime == default)
        {
            throw new ArgumentException("recurrenceModel.StartDateTime is default");
        }

        var cronExpression = $"{recurrenceModel.StartDateTime.Minute}";
        if (recurrenceModel.MinuteInterval != null)
        {
            cronExpression += @$"/{recurrenceModel.MinuteInterval}";
        }

        cronExpression += $" {recurrenceModel.StartDateTime.Hour}";
        if (recurrenceModel.HourInterval != null)
        {
            cronExpression += @$"/{recurrenceModel.HourInterval}";
        }

        cronExpression += $" {recurrenceModel.StartDateTime.Day}";
        if (recurrenceModel.MonthDayInterval != null)
        {
            cronExpression += @$"/{recurrenceModel.MonthDayInterval}";
        }

        cronExpression += $" {recurrenceModel.StartDateTime.Month} *";

        return cronExpression;
    }

    public static RecurrenceModel ToRecurrenceModel(this string cronExpression)
    {
        string[] timeUnits = cronExpression.Split(' ');
        int startDay;
        int startHour;
        int startMinute;
        byte? monthDayInterval;
        byte? hourInterval;
        byte? minuteInterval;
        string day = timeUnits[2];
        string hour = timeUnits[1];
        string minute = timeUnits[0];
        if (day.Contains('/'))
        {
            startDay = int.Parse(day.Split('/')[0]);
            monthDayInterval = byte.Parse(day.Split('/')[1]);
        }
        else
        {
            startDay = int.Parse(day);
            monthDayInterval = null;
        }

        if (hour.Contains('/'))
        {
            startHour = int.Parse(hour.Split('/')[0]);
            hourInterval = byte.Parse(hour.Split('/')[1]);
        }
        else
        {
            startHour = int.Parse(hour);
            hourInterval = null;
        }

        if (minute.Contains('/'))
        {
            startMinute = int.Parse(minute.Split('/')[0]);
            minuteInterval = byte.Parse(minute.Split('/')[1]);
        }
        else
        {
            startMinute = int.Parse(minute);
            minuteInterval = null;
        }

        var recurrenceModel = new RecurrenceModel
        {
            StartDateTime = new DateTime(
                DateTime.Now.Year,
                int.Parse(timeUnits[3]),
                startDay,
                startHour,
                startMinute,
                0),

            MonthDayInterval = monthDayInterval,
            HourInterval = hourInterval,
            MinuteInterval = minuteInterval,
        };

        return recurrenceModel;
    }

    public static DateTime GetNextAnalysisDateTime(this string cronExpression)
    {
        var recurrenceModel = cronExpression.ToRecurrenceModel();
        // var q = DateTime.Now.AddDays(recurrenceModel.MonthDayInterval) - DateTime.Now; // exec time - now time
        var nextDateTime = recurrenceModel.StartDateTime;
        // if (nextDateTime > DateTime.Now)
        // {
        //     return nextDateTime;
        // }
        //
        // var q1 = nextDateTime;
        // var q2 = nextDateTime;
        // var minutes = 0;
        // var hours = 0;
        // while (nextDateTime < DateTime.Now)
        // {
        //     nextDateTime = nextDateTime.AddMinutes(recurrenceModel.MinuteInterval ?? 0);
        //     minutes++;
        //
        //     if (minutes >= 60)
        //     {
        //         nextDateTime = q1;
        //         nextDateTime = nextDateTime.AddHours(recurrenceModel.HourInterval ?? 0);
        //         q1 = q1.AddHours(recurrenceModel.HourInterval ?? 0);
        //         hours++;
        //         minutes = 0;
        //     }
        //
        //     if (hours >= 24)
        //     {
        //         nextDateTime = q2;
        //         nextDateTime = nextDateTime.AddDays(recurrenceModel.MonthDayInterval ?? 0);
        //         q2 = q2.AddDays(recurrenceModel.MonthDayInterval ?? 0);
        //         hours = 0;
        //     }
        // }
        
        

        // var i = 1;
        // while (nextDateTime < DateTime.Now && i <= 60)
        // {
        //     i++;
        //     var nextDateTime1 = nextDateTime.AddMinutes(recurrenceModel.MinuteInterval ?? 0);
        // }

        // while (nextDateTime.Day < DateTime.Now.Day)
        // {
        //     nextDateTime = nextDateTime.AddDays(recurrenceModel.MonthDayInterval ?? 0);
        // }
        //
        // if (nextDateTime.Day == DateTime.Now.Day)
        // {
        //     while (nextDateTime.Hour < DateTime.Now.Hour)
        //     {
        //         nextDateTime = nextDateTime.AddHours(recurrenceModel.HourInterval ?? 0);
        //     }
        // }
        //
        // if (nextDateTime.Day != DateTime.Now.Day)
        // {
        //     while (nextDateTime.Hour < DateTime.Now.Hour)
        //     {
        //         nextDateTime = nextDateTime.AddHours(recurrenceModel.HourInterval ?? 0);
        //     }
        // }
        //
        // while (nextDateTime.Minute < DateTime.Now.Minute)
        // {
        //     nextDateTime = nextDateTime.AddMinutes(recurrenceModel.MinuteInterval ?? 0);
        // }

        return nextDateTime;
    }
}