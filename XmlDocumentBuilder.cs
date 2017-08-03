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
        /// Merges the DocumentElement of each XML file into a new XML file.
        /// </summary>
        /// <param name="xmlfiles">A list of string objects that are the XML file contents.</param>
        /// <returns>An XmlDocument object with all files combined as child nodes of root_tag.</returns>
        public static XmlDocument CombineXmlDocuments(List<string> xmlfiles, XmlDeclaration declaration = null)
        {
            var xmlmasterdocument = new XmlDocument();

            if (declaration != null)
            {
                xmlmasterdocument.InsertBefore(
                    declaration,
                    xmlmasterdocument.DocumentElement
                );
            }
            else
            {
                xmlmasterdocument.InsertBefore(
                    xmlmasterdocument.CreateXmlDeclaration("1.0", "utf-8", null),
                    xmlmasterdocument.DocumentElement
                );
            }

            foreach (var file in xmlfiles)
            {
                var partialxmldocument = new XmlDocument();
                partialxmldocument.LoadXml(file);
                xmlmasterdocument.DocumentElement.AppendChild(xmlmasterdocument.ImportNode(partialxmldocument.DocumentElement, true));
            }

            return xmlmasterdocument;
        }

        public static async Task<XmlDocument> CombineXmlDocuments(List<Task<string>> xmlfiles)
        {
            return CombineXmlDocuments((await Task.WhenAll(xmlfiles.ToArray())).ToList());
        }

        public static async Task<XmlDocument> CombineXmlDocuments(Task<string>[] xmlfiles)
        {
            return CombineXmlDocuments((await Task.WhenAll(xmlfiles)).ToList());
        }

        /// <summary>
        /// Combine an arbitrary number of XML documents that all have the same root tag.
        /// </summary>
        /// <param name="root_tag">The root tag shared among the XML documents.</param>
        /// <param name="xmlfiles">A list of string objects that are the XML file contents.</param>
        /// <returns>An XmlDocument object with all files combined as child nodes of root_tag.</returns>
        public static XmlDocument CombineXmlDocumentsByRoot(string root_tag, List<string> xmlfiles, XmlDeclaration declaration = null)
        {
            var xmlmasterdocument = new XmlDocument();

            if (declaration != null)
            {
                xmlmasterdocument.InsertBefore(
                    declaration,
                    xmlmasterdocument.DocumentElement
                );
            }
            else
            {
                xmlmasterdocument.InsertBefore(
                    xmlmasterdocument.CreateXmlDeclaration("1.0", "utf-8", null),
                    xmlmasterdocument.DocumentElement
                );
            }

            xmlmasterdocument.AppendChild(xmlmasterdocument.CreateElement(root_tag));
            var xmlmasterrootag = xmlmasterdocument.GetElementsByTagName(root_tag).Item(0);

            // Go through each file and append its child to the defines Xml document above.
            foreach (var file in xmlfiles)
            {
                var partialxmldocument = new XmlDocument();
                partialxmldocument.LoadXml(file);

                var childnodes = partialxmldocument
                    .GetElementsByTagName(root_tag)
                    .Item(0)
                    .ChildNodes;

                foreach (XmlNode xmlnode in childnodes)
                {
                    xmlmasterrootag.AppendChild(xmlmasterdocument.ImportNode(xmlnode, true));
                }
            }

            return xmlmasterdocument;
        }

        /// <summary>
        /// Combine an arbitrary number of XML documents that all have the same root tag. Supports async/await.
        /// </summary>
        /// <param name="root_tag">The root tag shared among the XML documents.</param>
        /// <param name="xmlfiles">A list of Task<string> objects that are the XML file contents.</param>
        /// <returns>An XmlDocument object with all files combined as child nodes of root_tag.</returns>
        public static async Task<XmlDocument> CombineXmlDocumentsByRoot(string root_tag, List<Task<string>> xmlfiles)
        {
            return CombineXmlDocumentsByRoot(root_tag, (await Task.WhenAll(xmlfiles.ToArray())).ToList());
        }

        /// <summary>
        /// Combine an arbitrary number of XML documents that all have the same root tag. Supports async/await.
        /// </summary>
        /// <param name="root_tag">The root tag shared among the XML documents.</param>
        /// <param name="xmlfiles">An array of Task<string> objects that are the XML file contents.</param>
        /// <returns>An XmlDocument object with all files combined as child nodes of root_tag.</returns>
        public static async Task<XmlDocument> CombineXmlDocumentsByRoot(string root_tag, Task<string>[] xmlfiles)
        {
            return CombineXmlDocumentsByRoot(root_tag, (await Task.WhenAll(xmlfiles)).ToList());
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
            using (var definesreader = new StringReader(xmlstring))
            {
                return (T)xmldefines_serializer.Deserialize(definesreader);
            }
        }
    }
}
