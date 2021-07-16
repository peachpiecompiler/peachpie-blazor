<?php
function CsvToString($csv)
{
	$result = "";
	foreach($csv as $line)
	{
		for ($i = 0; $i < count($line) - 1; $i++)
		{
			$result .= strval($line[$i]) . ",";
		}

		$result .= strval($line[count($line) - 1]) . "\n";
	}
	return $result;
}

$file = CreateFile(CsvToString($results),"text/csv", "results.csv");
DownLoadFile($file->id);
?>
<a href="Benchmark/main.php">New Benchmark</a>
