﻿<?php namespace Asteroids;

require_once(__DIR__ . "\asteroids.php");

#[\Microsoft\AspNetCore\Components\RouteAttribute("Asteroids")]
class AsteroidsComponent extends \Peachpie\Blazor\PhpComponent
{	
	private $app;
	private $previousTime;
	private $previousFrames;
	private $frames;
	private $framerate;
	private \Peachpie\Blazor\Timer $timer;

	private $infoTag;
	private $NewGameTag;

	public function BuildRenderTree($builder) : void 
	{
		$seq = $this->infoTag->writeWithTreeBuilder($builder, 0);

		$seq = $this->NewGameTag->writeWithTreeBuilder($builder, $seq);

		$this->app->writeWithTreeBuilder($builder, $seq);
		$this->timer->Start();
	}

	public function OnInitialized() : void 
	{
		parent::OnInitialized();

		require(__DIR__ . "/settings.php");
		$this->app = new Application($settings);	
		
		$this->timer = new \Peachpie\Blazor\Timer($settings["speed"]);
		$this->timer->addEventElapsed(function($s, $e) {$this->ClickHandler($s, $e);});
		$this->timer->AutoReset(false); 

		$this->framerate = 0;
		$this->infoTag = new \Peachpie\Blazor\Tag("p");
        $this->infoTag->content[] = new \Peachpie\Blazor\Text("FPS: " . strval(round($this->framerate, 1)));

		$this->NewGameTag = new \Peachpie\Blazor\Tag("a");
        $this->NewGameTag->attributes["href"] = "/Graph/index.php";
		$this->NewGameTag->content[] =  new \Peachpie\Blazor\Text("Restart");

		$this->previousTime = microtime(true);
		$this->previousFrames = previousTime;
	}

	public function ClickHandler($sender, $e)
	{
		$newTime = microtime(true);
		
		$delta = $newTime - $this->previousTime;
		$this->previousTime = $newTime;
		$this->frames += 1;

		if ($newTime - $this->previousFrames > 1)
		{
			$this->framerate = $this->frames / ($newTime - $this->previousFrames);
			$this->previousFrames = $newTime;
			$this->frames = 0;
			$this->infoTag->content[0] = new \Peachpie\Blazor\Text("FPS: " . strval(round($this->framerate, 1)));
		}

		$this->app->tick($delta);

		global $graph;
		$graph = $this->app->GetStats();
		
		$this->StateHasChanged();
	}

	public function Dispose() 
	{
		parent::Dispose();
		$this->timer->Dispose();
	}
}