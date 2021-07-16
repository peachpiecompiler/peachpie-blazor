<?php
$setting = [
    "fps" => intval($_GET["fps"]), // Frames Per Second
    "aps" => intval($_GET["aps"]), // Asteroids Per Second
    "asteroid_size" => intval($_GET["asteroid_size"]),
    "asteroid_speed" => intval($_GET["asteroid_speed"]), // Pixels per second
    "background_height" => intval($_GET["background_height"]),
    "background_width" => intval($_GET["background_width"]),
    "rendering" => $_GET["rendering"],
    "duration" => intval($_GET["duration"])
];
?>
<h2>Benchmark summary:</h2>
<table>
  <tr>
    <th>Frames Per Second</th>
    <th><?php echo $setting["fps"]; ?></th>
  </tr>
  <tr>
    <th>Asteroids Per Second</th>
    <th><?php echo $setting["aps"]; ?></th>
  </tr>
    <tr>
    <th>Asteroid size</th>
    <th><?php echo $setting["asteroid_size"]; ?></th>
  </tr>
  <tr>
    <th>Asteroid speed (Pixels Per Second)</th>
    <th><?php echo $setting["asteroid_speed"]; ?></th>
  </tr>
    <tr>
    <th>Background height</th>
    <th><?php echo $setting["background_height"]; ?></th>
  </tr>
  <tr>
    <th>Background width</th>
    <th><?php echo $setting["background_width"]; ?></th>
  </tr>
    <tr>
    <th>Rendering</th>
    <th><?php echo $setting["rendering"]; ?></th>
  </tr>
  <tr>
    <th>Duration</th>
    <th><?php echo $setting["duration"]; ?></th>
  </tr>
</table>

<a href="/Asteroids">Run</a>