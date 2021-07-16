<?php

// ---Entities---

abstract class Entity extends \Peachpie\Blazor\Tag
{
    protected array $position;
    protected array $size;

    public function __construct(array $position, array $size) 
    {
        parent::__construct("div");
        $this->position = $position;
        $this->size = $size;
    }

    public function getPosition() : array
    {
        return $this->position;
    }

    public function getSize() : array
    {
        return $this->size;
    }
}

abstract class MovableEntity extends Entity
{
    protected array $direction;

    public function __construct(array $position, array $direction, array $size) 
    {
        parent::__construct($position, $size);
        $this->direction = $direction;
    }

    public function move(float $time) : void
    {
        $this->position["x"] = $this->direction["x"] * $time + $this->position["x"]; 
        $this->position["y"] = $this->direction["y"] * $time + $this->position["y"]; 
    }

    public function moveWithBounderies(float $time, array $bounderies) : void
    {
        $this->move($time);
        $this->position["x"] = max(min($this->position["x"], $bounderies["max_x"]), $bounderies["min_x"]);
        $this->position["y"] = max(min($this->position["y"], $bounderies["max_y"]), $bounderies["min_y"]); 
    }

    public function changeDirection(array $direction) : void
    {
        $this->direction = $direction;
    }
}

class Background extends Entity 
{
    public function __construct(array $size) 
    {
        parent::__construct(["x" => 0, "y" => 0], $size);

        $this->attributes["class"]->add("asteroids-background");
             
        $this->attributes["style"]["height"] = $size["height"] . "px";
        $this->attributes["style"]["width"] = $size["width"] . "px";
        $this->attributes["style"]["top"] = "0px";
        $this->attributes["style"]["left"] = "0px";
    }

    public static function createDefault() : Background
    {
        return new self(["height" => 700, "width" => 700]);
    }
}

class Asteroid extends MovableEntity 
{
    public function __construct(array $position, array $direction, array $size)
    {
        parent::__construct($position, $direction, $size);

        $this->attributes["class"]->add("asteroids-asteroid");
             
        $this->attributes["style"]["height"] = $this->size["height"] . "px";
        $this->attributes["style"]["z-index"] = 1;
        $this->attributes["style"]["width"] = $this->size["width"] . "px";
        $this->attributes["style"]["top"] = $this->position["y"] . "px";
        $this->attributes["style"]["left"] = $this->position["x"] . "px";
        /*/
        $this->content[] = new \Peachpie\Blazor\Tag("div");
        $this->content[0]->content[] = new \Peachpie\Blazor\Tag("div");
        $this->content[0]->content[0]->content[] = new \Peachpie\Blazor\Tag("div");
        $this->content[] = new \Peachpie\Blazor\Tag("div");
        $this->content[1]->content[] = new \Peachpie\Blazor\Tag("div");
        $this->content[1]->content[0]->content[] = new \Peachpie\Blazor\Tag("div");
        $this->content[] = new \Peachpie\Blazor\Tag("div");
        $this->content[2]->content[] = new \Peachpie\Blazor\Tag("div");
        $this->content[2]->content[0]->content[] = new \Peachpie\Blazor\Tag("div");
        /**/
    }

    public static function createDefault(array $position) : Asteroid
    {
        return new self($position, ["x" => 0, "y" => 40], ["height" => 50, "width" => 50]);
    }

    public function move(float $time) : void
    {
        parent::move($time);
        $this->attributes["style"]["top"] = $this->position["y"] . "px";
        $this->attributes["style"]["left"] = $this->position["x"] . "px";
    }

    public function penetration(array $position, array $size) : bool
    {
        $endAsteroidX = $this->position["x"] + $this->size["width"];
        $endAsteroidY = $this->position["y"] + $this->size["height"];
        $endOtherX = $position["x"] + $size["width"];
        $endOtherY = $position["y"] + $size["height"];

        return !($endAsteroidX < $position["x"] || $endOtherX < $this->position["x"]) && !($endAsteroidY < $position["y"] || $endOtherY < $this->position["y"]);
    }
}

class Application extends \Peachpie\Blazor\Tag
{
    public $asteroids;
    private $background;
    private $nextAsteroid;

    private array $gameSettings;

    public function __construct(array $settings) 
    {
        parent::__construct("div");
        $this->gameSettings = $settings;
        
        $this->attributes["class"]->add("asteroids-app");
        $this->attributes["style"]["height"] = $this->gameSettings["background_height"] . "px";
        $this->attributes["style"]["width"] =  $this->gameSettings["background_width"] . "px";
        $this->attributes["tabindex"] = 0;
             
        $this->initGame();
    }

    public function initGame() : void
    {
        $this->asteroids = array();
        $this->background = new Background(["height" => $this->gameSettings["background_height"], "width" => $this->gameSettings["background_width"]]);
        $this->content[] = &$this->background; 
    }

    public function GetObjectCounts()
    {
        //return count($this->asteroids) * 9 + 1;
        return count($this->asteroids) + 1;
    }

    public function tick($delta) : void
    {
        // Move
        $bounderies = ["max_x" =>  $this->gameSettings["background_width"], "max_y" => $this->gameSettings["background_height"], "min_x"=> 0, "min_y"=> 0];
        
        foreach($this->asteroids as $entity)
        {
            $entity->moveWithBounderies($delta, $bounderies);
        }

        // Destroy asteroids
        $asteroidsDestroyed = array();

        foreach($this->asteroids as $keyAsteroid => $asteroid)
        {
            if ($asteroid->getPosition()["y"] >= $bounderies["max_y"] - $asteroid->getSize()["width"])
            {
                $asteroidsDestroyed[$keyAsteroid] = true;
            }
        }

        $this->asteroids = array_diff_key($this->asteroids, $asteroidsDestroyed);

        // Create asteroids
        $this->nextAsteroid += $delta;
        if ($this->nextAsteroid  >= (1.0 / $this->gameSettings["aps"]))
        {
            $this->nextAsteroid = 0;
            $asteroid = new Asteroid(["x" => rand (0, $this->gameSettings["background_width"] - $this->gameSettings["asteroid_size"]), "y"=> 0],
                                     ["x" => 0, "y" => $this->gameSettings["asteroid_speed"]],
                                     ["height" => $this->gameSettings["asteroid_size"], "width" => $this->gameSettings["asteroid_size"]]);
            $this->asteroids[] = $asteroid;
        }
    }

    public function writeWithTreeBuilder($builder, int $startIndex) : int
    {
        $builder->OpenElement($startIndex++, $this->name);

        $startIndex = $this->attributes->writeWithTreeBuilder($builder, $startIndex);

        $startIndex = $this->background->writeWithTreeBuilder($builder, $startIndex);

        foreach ($this->asteroids as $fragment)
        {
            $new = $fragment->writeWithTreeBuilder($builder, $startIndex);
        }

        $builder->CloseElement();

        return $new;
    }

    public function ToString()
    {
        $page = "";
        $page .= "<" . $this->name . " ";
        $page .= $this->attributes->ToString();
        $page .= ">";

        foreach ($this->content as $fragment)
        {
            $page .= $fragment->ToString();
        }

        foreach ($this->asteroids as $fragment)
        {
           $page .= $fragment->ToString();
        }

        $page .= "</". $this->name . ">";
        return $page;
    }
}
