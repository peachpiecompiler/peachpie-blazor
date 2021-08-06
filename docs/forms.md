# Forms

Web forms are a common way how to interact with a client using PHP. We wanted to move the form handling to a client side. So, web forms generated from PHP scripts are now handled on a client side without knowing about additional configuration. You just specify a request method and handling script. Superglobals `$_GET` and `$_POST` work as you expect.

> The example of using web forms can be found in [https://github.com/peachpiecompiler/peachpie-blazor/tree/master/src/Tests/Examples/OneScript](https://github.com/peachpiecompiler/peachpie-blazor/tree/master/src/Tests/Examples/OneScript)

When you generate a web form from a PHP script, `PhpScriptProvider` turns the form handling to a client side, where the provider finds the handling script of the form.

More information about the API can be found in [[https://docs.peachpie.io/scenarios/blazor/api-reference/]([https://docs.peachpie.io/scenarios/blazor/api-reference/).

## Files

Loading and downloading files are possible as well. However, we need specialized functions to manage it.

> The example of using web forms can be found in [https://github.com/peachpiecompiler/peachpie-blazor/tree/master/src/Tests/Examples/OneScript](https://github.com/peachpiecompiler/peachpie-blazor/tree/master/src/Tests/Examples/OneScript)

### Uploading file

When a client uploads a file to the webform and sends it, we can get information about sent files in superglobal `$_FILES`. When you want to read a file content, you have to use `GetBrowserFileContent($fileId)`, which returns Base64 encoded content of the file by the given id. This id has every object returned from `$_FILES`.

### Creating file

Because of the browser environment, you have to use the specialized function for creating a file. `CreateFile(string $data, $contentType, $name)` will create a browser representation of the file with given data, type, and name.

### Downloading file

Then you can download an already existing file by `DownloadFile($fileId)`, which finds the file in the memory and start to download it to a client PC.

You can see an example of creating a CSV file and downloading it to a client below.

```php
<?php

$file = CreateFile('Hello, world',"text/csv", "graph.csv");
DownloadFile($file->id);
```