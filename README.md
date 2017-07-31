# XmlDocument-Merger
Combines XmlDocument objects with the same root element.

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

There are two methods:
- XmlDocument CombineXmlDocuments(string root_tag, List<string> xmlfiles)
- async Task<XmlDocument> CombineXmlDocuments(string root_tag, List<Task<string>> xmlfiles)

The difference being that the second one supports async/await and returns Task\<XmlDocument\>, and the first one doesn't.

Limitations:
 - The XML Documents you are merging MUST have the same root element.
 - It will only do the first child tag of the root tag (because XML elements have to be imported, and .ImportNode only supports one XmlNode).
