# PHP JavaScript interoperability

Interoperability is an interesting part of integration. We are able to call JS from PHP and vice versa.

## Calling JavaScript

When we want to call the JS function, we just use prepared methods `CallJsVoid` or `CallJs`, which takes the method names, method parameters  and calls the desired function for us.

```php
<?php
    CallJsVoid("window.alert", "Hello from PHP script.");
?>
```

## Calling PHP

We can also call PHP function when there is an instance of `PhpScriptProvider` or `PhpComponent`. We just call `window.php.callPHP`, which takes the function name and its parameter as JSON structure.

We can see an example of usage in the example below.

```php
<p>Click and look at console output</p>
<button onclick="window.php.callPHP('CallPHP', { name : 'Bon', surname: 'Jovi'});">PHP</button>
<?php

function CallPHP($data)
{
    $json = json_decode($data); 

	echo "Hello ". $json->name . " " . $json->surname .  " from PHP\n";
}
?>
```