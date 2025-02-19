﻿using System;
using System.IO;
using PowerDocu.Common;

namespace PowerDocu.FlowDocumenter
{
    class FlowDocumenter
    {
        static void Main(string[] args)
        {
            NotificationHelper.AddNotificationReceiver(new ConsoleNotificationReceiver());
            if (args.Length == 0 || args.Length > 2)
            {
                NotificationHelper.SendNotification("Please provide an exported Flow package as parameter (mandatory), and optionally a Word document to use as template. For example:");
                NotificationHelper.SendNotification("  powerdocu.flowdocumenter.exe ExportedFlow.zip");
                NotificationHelper.SendNotification("  powerdocu.flowdocumenter.exe ExportedFlow.zip WordTemplate.docx");
            }
            else
            {
                if (args.Length == 1)
                    FlowDocumentationGenerator.GenerateDocumentation(args[0], "All");
                if (args.Length == 2)
                    FlowDocumentationGenerator.GenerateDocumentation(args[0], "All", args[1]);
            }
        }
    }
}
