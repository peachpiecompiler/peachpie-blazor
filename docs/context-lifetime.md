# PHP context lifetime

PHP context usually terminates with the PHP script execution. It is the standard behavior of PHP. However, we can utilize PeachPie and Blazor to save the context on a client side to save the application state.

> An example can be seen in [https://github.com/peachpiecompiler/peachpie-blazor/tree/dev/docs/src/Tests/Examples/AllTogether](https://github.com/peachpiecompiler/peachpie-blazor/tree/dev/docs/src/Tests/Examples/AllTogether), where we utilize the persistent context for saving data from many parts of the application representing different PHP scripts.

You will change the context lifetime of the provider by setting the `ContextLifetime` parameter to  `SessionLifetime.Persistant`. When a client navigates to another PHP script, the context of this script is the same as the previous one. 

> The current implementation doesn't support more than one context at a time. So when there are two instances of the provider with different `ContextLifetime`, the context persistency will not work.

```razor
<PhpScriptProvider ContextLifetime="@SessionLifetime.Persistant" Type="@PhpScriptProviderType.ScriptProvider">
    <Navigating>
        <p>Navigating</p>
    </Navigating>
    <NotFound>
        <p>Not found</p>
    </NotFound>
</PhpScriptProvider>
```