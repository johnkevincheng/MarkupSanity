# MarkupSanity
Use Html Agility Pack parser to sanitize html text against unrecognized tags and attributes.

As with any input processing, Markup Sanity adds performance degradations to the process, and in this case, quite significant due to the dependence on parsing from Html Agility Pack.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

At its simplest usage, MarkupSanity is an extension method for Strings and validates the value against a pre-defined set of default whitelisted tags and attrbutes.

As an example,

    String inputValue = "<a href=\"javascript:alert('Gotcha again!');\" onclick=\"javascript:alert('Gotcha!');\">Click Me</a>";
    String cleanValue = inputValue.SanitizeHtml();
    Console.Writeline(cleanValue);

The output shall be:

    <a>Click Me</a>

The `onclick` attribute is removed due to its use for scripting. But while `href` is a valid attribute, its usage to trigger a Javascript alert is considered dangerous and thus removed.

You may refer to the [wiki](https://github.com/johnkevincheng/MarkupSanity/wiki) for more advanced usage information.

### Prerequisites

* [Html Agility Pack](https://github.com/zzzprojects/html-agility-pack)
* .NET Framework 3.5

### Installing

Add the MarkupSanity project to your .NET solution, and have your relevant projects reference this class library to be able to access the classes and extension methods needed to process html texts.

Alternatively, you may download the dll package directly from Nuget [here](https://www.nuget.org/packages/RockFluid.MarkupSanity/).

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/johnkevincheng/MarkupSanity/tags). 

## Authors

* **Kevin Cheng** - *Initial work* - [johnkevincheng](https://github.com/johnkevincheng)

See also the list of [contributors](https://github.com/johnkevincheng/MarkupSanity/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
