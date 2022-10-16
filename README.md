# HtmlObjectBuilder

Uses HtmlAgilityPack and reflection to effortlessly instantiate C# classes from HTML.

## Purpose:

It's really tedious trying to parse through a bunch of HTML to get the data you want to use in your application. Trying to do so can easily lead to messy unmanagable code. This library uses a custom Attribute called HtmlProperty, which allows you to easily identify a specific piece of information from HTML and store the value in the corresponding property. By doing this all of the leg work is compeltely removed, making it much easier to populate values from HTML into a class.

## How it works:

Lets say you have a class Person that you want to create from the HTML below.
```cs
class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsHungry { get; set; }
}
```

```html

<html>
  <body>
    <h1 class ="firstName">John</h1>
    <p class="text-semibold age">35</p>
    <h2 class="text-bold isHungry">true</h2>
  </body>
</html>

```

You can use the HtmlProperty attribute to specify which tags have the info you want. This is done in 3 steps:
1. Inherit the HtmlObject base class
2. Override one of the constructors and pass in either the object's html or an HtmlNode (from HtmlAgilityPack)
3. Use HtmlProperty on your class's Properties to allow them to be automatically assigned by the library.

Example:
```cs
class Person : HtmlObject
{
    [HtmlProperty(hasClass: "firstName")]
    public string Name { get; set; }

    [HtmlProperty(hasClass: "age")]
    public int Age { get; set; }

    [HtmlProperty(hasClass: "isHungry")]
    public bool IsHungry { get; set; }

    public Person(string objectHtml) : base(objectHtml)
    {
    }
}
```
Using the example above, you can instantiate an object from the original html:
```cs
var person1 = new Person(html);
Console.WriteLine(person1.Name); // John
Console.WriteLine(person1.Age); // 35
Console.WriteLine(person1.IsHungry); // true
```

## Find tags with multiple classes
You can search for tags with multiple classes using an Array of Strings.

Example:
```cs
class Person : HtmlObject
{
    [HtmlProperty(classes: new string[] { "firstName" })]
    public string Name { get; set; }

    [HtmlProperty(classes: new string[] { "text-semibold", "age" })]
    public int Age { get; set; }

    [HtmlProperty(classes: new string[] { "text-bold", "isHungry" })]
    public bool IsHungry { get; set; }

    public Person(string objectHtml) : base(objectHtml)
    {
    }
}
```

## Some notes:
- Will work on Public and Private properties.
- Will work whether or not the tag is deeply nested.
- You don't need to specify every class the html property has, just enough to identify it.
- Make sure you specify which html class or classes the property will have.
- Make sure there are no tags before your desired one with duplicate html classes.

## Future improvements:
- Allow library to detect tags with duplicate classes and find the correct one.
- Allow library to find tag based on path, attributes, or other info in html.
