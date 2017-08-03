# XmlDocument-Merger
Combines XmlDocument objects with the same root element. Targets .NET Framework 4.5.

This will take something like this:

```xml
<Root>
  <Tag1>Hello World!</Tag1>
</Root>
```

And merge it with this:

```xml
<Root>
  <Tag2>Hello World! The Sequel</Tag2>
</Root>
```

To create this:

```xml
<Root>
  <Tag1>Hello World!</Tag1>
  <Tag2>Hello World! The Sequel</Tag2>
</Root>
```

There are three methods:
- XmlDocument CombineXmlDocumentsByRoot(string root_tag, List\<string\> xmlfiles, XmlDeclaration declaration = null)
- async Task\<XmlDocument\> CombineXmlDocumentsByRoot(string root_tag, List\<Task\<string\>\> xmlfiles, XmlDeclaration declaration = null)
- async Task\<XmlDocument\> CombineXmlDocumentsByRoot(string root_tag, Task\<string\>[] xmlfiles, XmlDeclaration declaration = null)

The difference being that the second one supports async/await and returns Task\<XmlDocument\>, and the first one doesn't. The third one is an overloaded version that takes an array rather than a list.

There is also another version, CombineXmlDocuments, that simply combines the XML documents by DocumentElement.
