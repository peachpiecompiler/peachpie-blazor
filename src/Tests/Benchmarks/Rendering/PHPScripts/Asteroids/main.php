﻿<?php namespace Asteroids;

#[\Microsoft\AspNetCore\Components\RouteAttribute("Asteroids")]
class AsteroidsComponent extends \Peachpie\Blazor\PhpComponent
{	
	private \Peachpie\Blazor\Timer $timer;
	private $app;
	private $startTime;
	private $previousTick;
	private $frames;
	private $infoTag;
	private $framerate;
	private $setting;
	private $end;
	private $logs;
	private $onesecond;

	public function BuildRenderTree($builder) : void 
	{
		if ($this->end)
		{
			global $results;
			$results = $this->logs;
			$builder->AddMarkupContent(0, "<a href=\"Benchmark/downloadResults.php\">Download Results</a>");
			return;
		}

		$this->frames += 1;

		if ($this->setting["rendering"] === "simple")
		{
			$content = $this->infoTag->ToString();
			$content .= $this->app->ToString();
			$builder->AddMarkupContent(0, $content);
		}
		elseif ($this->setting["rendering"] === "complex") 
		{
			$seq = $this->infoTag->writeWithTreeBuilder($builder, 0);
			$this->app->writeWithTreeBuilder($builder, $seq);
		}

		$this->timer->Start();
	}

	public function OnInitialized() : void 
	{
		parent::OnInitialized();
		
		global $setting;

		//Timer
		$this->timer = new \Peachpie\Blazor\Timer(1000 / $setting["fps"]);
		$this->timer->addEventElapsed(function($s, $e) {$this->TickHandler($s, $e);});
		$this->timer->AutoReset(false); 

		$this->app = new \Application($setting);			
		$this->infoTag = new \Peachpie\Blazor\Tag("p");
		$this->infoTag->content[] = new \Peachpie\Blazor\Text("");

		$this->setting = $setting;
		$this->end = false;
		$this->logs = array();
		$this->logs[] = [0 => "fps", 1 => "time", 2 => "objects"];
		$frames = 0;
		$this->startTime = microtime(true);
		$this->previousTick = $this->startTime;
	}

	public function GetStatsString()
	{
		return "FPS: " . strval(round($this->framerate,1)) . "\n" . "Objects: " . strval($this->app->GetObjectCounts()) . "\n" . "Time: " . strval(round($this->previousTime - $this->startTime,1));
	}

	public function TickHandler($sender, $e)
	{
		$newTime = microtime(true);
		$delta = $newTime - $this->previousTime;
		$this->previousTime = $newTime;

		if ($newTime - $this->startTime > $this->setting["duration"])
		{
			$this->end = true;
			$this->timer->Stop();
			$this->timer->Dispose();
		}

		$this->onesecond += $delta;
		if ($this->onesecond >= 1)
		{
			$this->framerate = $this->frames / $this->onesecond;
			$this->onesecond = 0;	
			$this->frames = 0;

			$this->infoTag->content[0] = new \Peachpie\Blazor\Text($this->GetStatsString());

			$this->logs[] = [0 => $this->framerate, 1 => ($newTime - $this->startTime), 2 => $this->app->GetObjectCounts()];
		}

		$this->app->tick($delta);
		$this->StateHasChanged();
	}

	public function Dispose()
	{
		parent::Dispose();
		$this->timer->Stop();
		$this->timer->Dispose();
	}
}