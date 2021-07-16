<?php
function printGraphView()
{
	global $graph;
	
	require("/Graph/showGraph.php");
	printGraph($graph);
	unset($graph);
	require("/Graph/NewGame.php");
}

if (isset($graph)) {
	printGraphView();
}
else {
	require("/Graph/NewGame.php");
}
