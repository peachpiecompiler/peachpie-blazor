<?php

function bench_csv() 
{
	$csv = "a,b,c,d\n1,2,3,4\n1,2,3,4";
	$graph = array();
	$time = array();

	for ($i = 0; $i < 100; $i++)
	{
		$started = microtime(true);
		$graph = str_getcsv($csv);
		$end = microtime(true);
		if ($i >= 50)
			$time[] = $end - $started;
	}

	return array_sum($time)/count($time);
}

function bench_gd() 
{
	$time = array();
	for ($i = 0; $i < 100; $i++)
	{
		$started = microtime(true);
		$redimg = imagecreatetruecolor(1920, 1080);
		$end = microtime(true);
		//if ($i >= 50)
		return $end - $started;
	}

	return array_sum($time)/count($time);
}

function bench_md5() 
{	
	$csv = "a,b,c,d\n1,2,3,4\n1,2,3,4";
	$res = "";
	$time = array();

	for ($i = 0; $i < 100; $i++)
	{
		$started = microtime(true);
		$res = md5($csv);
		$end = microtime(true);
		//if ($i >= 50)
		return $end - $started;
	}

	return array_sum($time)/count($time);
}

function printResult($value, $function)
{
	echo "<p>\n";
	echo "The function <b>$function;</b>\n";
	echo "Total elapsed time: $value\n";
	echo "</p>";
}


printResult(round(bench_csv(),7), "str_getcsv(\$csv)"); 
printResult(round(bench_gd(),7), "imagecreatetruecolor(1920, 1080)"); 
//printResult(round(bench_md5(),7), "md5(\$csv)"); 