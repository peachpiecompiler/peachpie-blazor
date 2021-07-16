<?php namespace Asteroids;

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

class Bullet extends MovableEntity 
{
    public function __construct(array $position, array $direction, array $size)
    {
        parent::__construct($position, $direction, $size);

        $this->attributes["class"]->add("asteroids-bullet");
             
        $this->attributes["style"]["height"] = $this->size["height"] . "px";
        $this->attributes["style"]["z-index"] = 1;
        $this->attributes["style"]["width"] = $this->size["width"] . "px";
        $this->attributes["style"]["top"] = $this->position["y"] . "px";
        $this->attributes["style"]["left"] = $this->position["x"] . "px";
    }

    public function move(float $time) : void
    {
        parent::move($time);
        $this->attributes["style"]["top"] = $this->position["y"] . "px";
        $this->attributes["style"]["left"] = $this->position["x"] . "px";
    }

    public static function createDefault(array $position)
    {
        return new self($position, ["x" => 0, "y" => -60],  ["height" => 20, "width" => 10]);
    }
}

class Rocket extends MovableEntity
{
    public function __construct(array $position, array $direction, array $size)
    {
        parent::__construct($position, $direction, $size);

        $this->attributes["class"]->add("asteroids-rocket");
             
        $this->attributes["style"]["height"] = $this->size["height"] . "px";
        $this->attributes["style"]["z-index"] = 1;
        $this->attributes["style"]["width"] = $this->size["width"] . "px";
        $this->attributes["style"]["top"] = $this->position["y"] . "px";
        $this->attributes["style"]["left"] = $this->position["x"] . "px";
    }

    public static function createDefault(array $position)
    {
        $width = 40;
        $height = 40;

        return new self($position, ["x" => 0, "y" => 0], ["height" => $height, "width" => $width]);
    }

    public function move(float $time) : void
    {
        parent::move($time);
        $this->attributes["style"]["top"] = $this->position["y"] . "px";
        $this->attributes["style"]["left"] = $this->position["x"] . "px";
    }
}

class Application extends \Peachpie\Blazor\Tag 
{
    private $rocket;
    private $bullets;
    private $asteroids;
    private $backgroung;
    private $nextAsteroid;
    private $ended;
    private $stats;

    private array $gameSettings;

    public function __construct(array $settings) 
    {
        parent::__construct("div");
        $this->gameSettings = $settings;
        
        $this->attributes["class"]->add("asteroids-app");
        $this->attributes["style"]["height"] = $this->gameSettings["height"] . "px";
        $this->attributes["style"]["width"] = $this->gameSettings["width"] . "px";
        $this->attributes["tabindex"] = 0;
             
        $this->attributes->addEvent("onkeydown", function($seq, $builder) {$builder->AddEventKeyboardCallback($seq, "onkeydown", function($e) {$this->HandleKeyDown($e);});});
        $this->attributes->addEvent("onkeyup", function($seq, $builder) {$builder->AddEventKeyboardCallback($seq, "onkeyup", function($e) {$this->HandleKeyUp();});});

        $this->addButtons();
        $this->initGame();
    }

    public function isEnded() : bool 
    {
        return $this->ended;
    }

    public function GetStats()
    {
        $result = array();
        
        for($i = 0 ; $i < 20 ; $i++)
        {
            $result[] = [ 0 => rand(0,200), 1 => rand(0,200)];
        }

        return $result;
    }
    
    public function addButtons() : void
    {
        $button= new \Peachpie\Blazor\Tag("button");
        $button->attributes["style"]["position"] = "absolute";
        $button->attributes["style"]["top"] = "700px";
        $button->attributes["style"]["left"] = "0px";
               
        $button->attributes->addEvent("onmousedown", function($seq, $builder) {$builder->AddEventMouseCallback($seq, "onmousedown", function($e) {$this->HandleMouseDownMoveRight();});});
        $button->attributes->addEvent("onmouseup", function($seq, $builder) {$builder->AddEventMouseCallback($seq, "onmouseup", function($e) {$this->HandleMouseUp();});});
               
        $button->content[] = new \Peachpie\Blazor\Text("Move Right");

        $this->content[] = $button;
        unset($button);

        $button= new \Peachpie\Blazor\Tag("button");
        $button->attributes["style"]["position"] = "absolute";
        $button->attributes["style"]["top"] = "700px";
        $button->attributes["style"]["left"] = "100px";
        
        $button->attributes->addEvent("onclick", function($seq, $builder) {$builder->AddEventMouseCallback($seq, "onclick", function($e) {$this->HandleFire();});});
        
        $button->content[] = new \Peachpie\Blazor\Text("Fire");

        $this->content[] = $button;
        unset($button);

        $button= new \Peachpie\Blazor\Tag("button");
        $button->attributes["style"]["position"] = "absolute";
        $button->attributes["style"]["top"] = "700px";
        $button->attributes["style"]["left"] = "200px";
               
        $button->attributes->addEvent("onmousedown", function($seq, $builder) {$builder->AddEventMouseCallback($seq, "onmousedown", function($e) {$this->HandleMouseDownMoveLeft();});});
        $button->attributes->addEvent("onmouseup", function($seq, $builder) {$builder->AddEventMouseCallback($seq, "onmouseup", function($e) {$this->HandleMouseUp();});});
               
        $button->content[] = new \Peachpie\Blazor\Text("Move Left");

        $this->content[] = $button;
        unset($button);
    }

    public function initGame() : void
    {
        $this->bullets = array();
        $this->asteroids = array();
        $this->background = Background::createDefault();
        $this->content[] = &$this->background; 

        // 40 is a default height of rocket
        $this->rocket = Rocket::createDefault(["x" => 0, "y" => $this->gameSettings["height"] - 140 ]);
        $this->content[] = &$this->rocket;
        $this->ended = false;
    }

    public function tick($delta) : void
    {
        // Move
        $max_x = $this->gameSettings["width"] - $this->rocket->getSize()["width"];
        $bounderies = ["max_x" => $max_x, "max_y" => $this->gameSettings["height"] - 150, "min_x"=> 0, "min_y"=> 0];
        
        foreach($this->asteroids as $entity)
        {
            $entity->moveWithBounderies($delta, $bounderies);
        }
        foreach($this->bullets as $entity)
        {
            $entity->move($delta, $bounderies);
        }
        $bounderies["max_y"]+=10;
        $this->rocket->moveWithBounderies($delta,$bounderies);

        // Destroy asteroids and bullets
        $bulletsDestroyed = array();
        $asteroidsDestroyed = array();

        foreach($this->bullets as $keyBullet => $bullet)
        {
            foreach($this->asteroids as $keyAsteroid => $asteroid)
            {
                if ($asteroid->penetration($bullet->getPosition(), $bullet->getSize()))
                {
                    $asteroidsDestroyed[$keyAsteroid] = true;
                    $bulletsDestroyed[$keyBullet]= true;
                    break;
                }
            }

            if ($bullet->getPosition()["y"] <= 0)
            {
                $bulletsDestroyed[$keyBullet]= true;
            }
        }

        foreach($this->asteroids as $keyAsteroid => $asteroid)
        {
            if ($asteroid->getPosition()["y"] >= 650)
            {
                $asteroidsDestroyed[$keyAsteroid] = true;
                $this->ended = true;
            }
        }

        $this->bullets = array_diff_key($this->bullets, $bulletsDestroyed);
        $this->asteroids = array_diff_key($this->asteroids, $asteroidsDestroyed);

        // Create asteroids
        $this->nextAsteroid += $delta;
        if ($this->nextAsteroid  > $this->gameSettings["asteroidFrequency"])
        {
            $this->nextAsteroid = 0;
            $asteroid = Asteroid::createDefault(["x" => rand (0, $this->gameSettings["width"] - 50), "y"=> 0]);
            $this->asteroids[] = $asteroid;
        }
    }

    private function HandleKeyDown($e) : void
    {
        if ($e->Key === "ArrowLeft")
        {
            $this->rocket->changeDirection(["x" => -50,"y" => 0]);
        }
        else if ($e->Key === "ArrowRight")
        {
            $this->rocket->changeDirection(["x" => 50,"y" => 0]);
        }
        else if ($e->Key === "f")
        {
            $bullet = Bullet::CreateDefault($this->rocket->GetPosition());
            $this->bullets[] = $bullet;
        }
    }

    private function HandleKeyUp() : void
    {
         $this->rocket->changeDirection(["x" => 0,"y" => 0]);
    }

    private function HandleMouseDownMoveRight() : void
    {
         $this->rocket->changeDirection(["x" => 40,"y" => 0]);
    }

    private function HandleMouseUp() : void
    {
         $this->rocket->changeDirection(["x" => 0,"y" => 0]);
    }

    private function HandleMouseDownMoveLeft() : void
    {
         $this->rocket->changeDirection(["x" => -50,"y" => 0]);
    }

    private function HandleFire() : void
    {
        $bullet = Bullet::CreateDefault($this->rocket->GetPosition());
        $this->bullets[] = $bullet;
    }

    public function writeWithTreeBuilder($builder, int $startIndex) : int
    {
        $builder->OpenElement($startIndex++, $this->name);

        $startIndex = $this->attributes->writeWithTreeBuilder($builder, $startIndex);

        $startIndex = $this->rocket->writeWithTreeBuilder($builder, $startIndex);
        $startIndex = $this->background->writeWithTreeBuilder($builder, $startIndex);

        foreach ($this->content as $fragment)
        {
            $startIndex = $fragment->writeWithTreeBuilder($builder, $startIndex);
        }

        foreach ($this->asteroids as $fragment)
        {
            $startIndex = $fragment->writeWithTreeBuilder($builder, $startIndex);
        }

        foreach ($this->bullets as $fragment)
        {
            $startIndex = $fragment->writeWithTreeBuilder($builder, $startIndex);
        }

        $builder->CloseElement();

        return $startIndex;
    }
}
