using CleanModels.Queries.Base;
using NLog;

namespace OsService.Queries;

/// <inheritdoc />
public class UsbHardwareQuery : CommandLineQueryBase<string>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> ExecuteCoreAsync() // for current connected: lsusb   |   dmesg | grep usb
    {
        string connectedUsbDevices = await ExecuteBashAsync("sudo cat /var/log/syslog | grep 'New USB device found'");
        Logger.Info(connectedUsbDevices);

        return GetSuccessfulResult(connectedUsbDevices);
    }
}