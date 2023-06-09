namespace CleanModels.Schedule;

public class RecurrenceModel
{
    public DateTime StartDateTime { get; set; }

    public byte? MonthDayInterval { get; set; }

    public byte? HourInterval { get; set; }

    public byte? MinuteInterval { get; set; }

    // public DateTime EndDateTime { get; set; }
}