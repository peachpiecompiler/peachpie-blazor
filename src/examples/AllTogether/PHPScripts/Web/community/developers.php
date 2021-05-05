<?php
    require("/Web/headers/defaultHeader.php");
?>

<?php
if (isset($_GET["developer"])) { 
    $name = $_GET["developer"];
    require("/Web/community/developer$name.php");
} else {
?>
<h1>Developers</h1>
<p>Something about developers...</p>
<p>You can also use anchors and query parameters as usual. PhpBlazor will try to find the script or PhpComponent.</p>
<p>Get more info about <a href="/Web/community/developers.php?developer=Richard">Richard</a>.</p>
<p>Get more info about <a href="/Web/community/developers.php?developer=Danesh">Danesh</a>.</p>
<?php
}
?>

<?php
    require("/Web/footers/defaultFooter.php");
?>


