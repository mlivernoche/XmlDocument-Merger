using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

// Targets .NET Framework 4.5

namespace XmlDocumentMerger
{
    public static class XmlDocumentBuilder
    {
        /// <summary>
        /// Combine an arbitrary number of XML documents that all have the same root tag.
        /// </summary>
        /// <param name="root_tag">The root tag shared among the XML documents.</param>
        /// <param name="xmlfiles">A list of string objects that are the XML file contents.</param>
        /// <returns>An XmlDocument object with all files combined as child nodes of root_tag.</returns>
        public static XmlDocument CombineXmlDocuments(string root_tag, List<string> xmlfiles)
        {
            var xmldefinesdocument = new XmlDocument();
            xmldefinesdocument.InsertBefore(
                xmldefinesdocument.CreateXmlDeclaration("1.0", "utf-8", null),
                xmldefinesdocument.DocumentElement
            );
            xmldefinesdocument.AppendChild(xmldefinesdocument.CreateElement(root_tag));

            // Go through each file and append its child to the defines Xml document above.
            foreach (var file in xmlfiles)
            {
                var partialxmldocument = new XmlDocument();
                partialxmldocument.LoadXml(file);

                // What this monstrosity does:
                // - Get the first <root_tag> tag in the xmldefinesdocument
                // - AppendChild that is
                //   - Imported Node that is
                //     - The FirstChild of the first <root_tag> in xmldocument (.ImportNode does not support ChildNodes).
                xmldefinesdocument
                    .GetElementsByTagName(root_tag)
                    .Item(0)
                    .AppendChild(
                        xmldefinesdocument
                            .ImportNode(
                                partialxmldocument
                                    .GetElementsByTagName(root_tag)
                                    .Item(0)
                                    .FirstChild,
                                true));
            }

            return xmldefinesdocument;
        }

        /// <summary>
        /// Combine an arbitrary number of XML documents that all have the same root tag. Supports async/await.
        /// </summary>
        /// <param name="root_tag">The root tag shared among the XML documents.</param>
        /// <param name="xmlfiles">A list of Task<string> objects that are the XML file contents.</param>
        /// <returns>An XmlDocument object with all files combined as child nodes of root_tag.</returns>
        public static async Task<XmlDocument> CombineXmlDocuments(string root_tag, List<Task<string>> xmlfiles)
        {
            return CombineXmlDocuments(root_tag, (await Task.WhenAll(xmlfiles.ToArray())).ToList());
        }

        /// <summary>
        /// Deserializes a string of XML into an object using XmlSerializer.
        /// </summary>
        /// <typeparam name="T">The type created from the XML document.</typeparam>
        /// <param name="xmlstring">The string contents of the XML document.</param>
        /// <returns>An object of type T.</returns>
        public static T DeserializeXmlString<T>(string xmlstring)
        {
            var xmldefines_serializer = new XmlSerializer(typeof(T));
            {
                using (var definesreader = new StringReader(xmlstring))
                {
                    return (T)xmldefines_serializer.Deserialize(definesreader);
                }
            }
        }
    }
}
