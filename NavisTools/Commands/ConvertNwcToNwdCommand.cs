using NavisTools.Interfaces;
using Autodesk.Navisworks.Api;
using System;
using System.IO;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Commands
{
    public class ConvertNwcToNwdCommand : ToolCommandBase
    {
        public const string Id = "ID_Button_ConvertNwcToNwd";

        public ConvertNwcToNwdCommand(
            IDocumentProvider documentProvider,
            ISelectionService selectionService,
            INotificationService notificationService)
            : base(documentProvider, selectionService, notificationService)
        {
        }

        public override string CommandId => Id;

        public override bool CanExecute => true;

        public override int Execute()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Navisworks Cache Files (*.nwc)|*.nwc";
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select NWC Files to Convert to NWD";

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return 0;
                }

                var files = openFileDialog.FileNames;
                if (files == null || files.Length == 0)
                {
                    return 0;
                }

                int successCount = 0;
                int failCount = 0;

                var document = Application.MainDocument;
                if (document == null)
                {
                    NotificationService.ShowError("Unable to access the active Navisworks document.");
                    return 0;
                }

                var originalFile = document.CurrentFileName;
                var exportOptions = new NwdExportOptions();

                foreach (var inputFile in files)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(inputFile) || !File.Exists(inputFile))
                        {
                            throw new FileNotFoundException("Input file was not found.", inputFile);
                        }

                        var inputPath = inputFile;
                        var outputPath = Path.ChangeExtension(inputPath, ".nwd");

                        document.OpenFile(inputPath);
                        document.ExportToNwd(outputPath, exportOptions);
                        document.Clear();

                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        NotificationService.ShowError($"Failed to convert {Path.GetFileName(inputFile)}: {ex.Message}");
                    }
                }

                var restoredOriginalModel = false;

                try
                {
                    if (!string.IsNullOrWhiteSpace(originalFile) && File.Exists(originalFile))
                    {
                        document.OpenFile(originalFile);
                        restoredOriginalModel = true;
                    }
                }
                catch (Exception ex)
                {
                    NotificationService.ShowWarning($"Conversion finished, but failed to reopen the original model: {ex.Message}", "Restore Model Failed");
                }

                if (!restoredOriginalModel)
                {
                    document.Clear();
                }

                if (successCount > 0)
                {
                    NotificationService.ShowInfo(
                        $"Successfully converted {successCount} file(s) to NWD.{(failCount > 0 ? $" {failCount} file(s) failed." : "")}",
                        "Conversion Complete");
                }
                else if (failCount > 0)
                {
                    NotificationService.ShowError($"Failed to convert any files. {failCount} file(s) failed.");
                }
            }

            return 0;
        }
    }
}
