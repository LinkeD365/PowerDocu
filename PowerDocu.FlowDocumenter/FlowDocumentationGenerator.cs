using System;
using System.Collections.Generic;
using System.IO;
using PowerDocu.Common;

namespace PowerDocu.FlowDocumenter
{
    public static class FlowDocumentationGenerator
    {
        public static List<FlowEntity> GenerateDocumentation(string filePath, string fileFormat, string wordTemplate = null)
        {
            if (File.Exists(filePath))
            {
                string path = Path.GetDirectoryName(filePath);
                DateTime startDocGeneration = DateTime.Now;
                FlowParser flowParserFromZip = new FlowParser(filePath);
                if (flowParserFromZip.packageType == FlowParser.PackageType.SolutionPackage)
                {
                    path += @"\Solution " + CharsetHelper.GetSafeName(Path.GetFileNameWithoutExtension(filePath));
                }
                List<FlowEntity> flows = flowParserFromZip.getFlows();
                foreach (FlowEntity flow in flows)
                {
                    GraphBuilder gbzip = new GraphBuilder(flow, path);
                    gbzip.buildTopLevelGraph();
                    gbzip.buildDetailedGraph();
                    FlowDocumentationContent content = new FlowDocumentationContent(flow, path);
                    if (fileFormat.Equals(OutputFormatHelper.Word) || fileFormat.Equals(OutputFormatHelper.All))
                    {
                        NotificationHelper.SendNotification("Creating Word documentation");
                        if (String.IsNullOrEmpty(wordTemplate) || !File.Exists(wordTemplate))
                        {
                            FlowWordDocBuilder wordzip = new FlowWordDocBuilder(content, null);
                        }
                        else
                        {
                            FlowWordDocBuilder wordzip = new FlowWordDocBuilder(content, wordTemplate);
                        }
                    }
                    if (fileFormat.Equals(OutputFormatHelper.Markdown) || fileFormat.Equals(OutputFormatHelper.All))
                    {
                        NotificationHelper.SendNotification("Creating Markdown documentation");
                        FlowMarkdownBuilder markdownFile = new FlowMarkdownBuilder(content);
                    }
                }
                DateTime endDocGeneration = DateTime.Now;
                NotificationHelper.SendNotification("FlowDocumenter: Created documentation for " + filePath + ". A total of " + flowParserFromZip.getFlows().Count + " files were processed in " + (endDocGeneration - startDocGeneration).TotalSeconds + " seconds.");
                return flows;
            }
            else
            {
                NotificationHelper.SendNotification("File not found: " + filePath);
            }
            return null;
        }
    }
}