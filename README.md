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

Limitations:
 - The XML Documents you are merging MUST have the same root element.
 - It will only do the first child tag of the root tag (because XML elements have to be imported, and .ImportNode only supports one XmlNode).
