using System.Diagnostics;
using System.Text;

namespace CleanModels.Queries.Base;

/// <inheritdoc />
public abstract class CommandLineQueryBase<TResult> : NonDbQueryBase<TResult>
{
    /// <summary>
    /// Execute bash command.
    /// </summary>
    /// <param name="args">Args.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    protected async Task<string> ExecuteBashAsync(string args)
    {
        const int timeout = 180_000;
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "bash",
            Arguments = "-c \"" + args + "\"",
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        var process = new Process
        {
            StartInfo = processStartInfo,
        };

        var result = new StringBuilder();
        using var outputWaitHandle = new AutoResetEvent(false);
        using var errorWaitHandle = new AutoResetEvent(false);
        process.OutputDataReceived += OnDataReceived(result, outputWaitHandle);
        process.ErrorDataReceived += OnDataReceived(result, errorWaitHandle);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        if (process.WaitForExit(timeout) && outputWaitHandle.WaitOne(timeout) && errorWaitHandle.WaitOne(timeout))
        {
            return result.ToString();
        }

        throw new Exception("Program timed out");
    }

    private static DataReceivedEventHandler? OnDataReceived(StringBuilder output, EventWaitHandle waitHandle)
    {
        return (_, e) =>
        {
            if (e.Data != null)
            {
                output.AppendLine(e.Data);
            }

            if (e.Data == null)
            {
                waitHandle.Set();
            }
        };
    }
}