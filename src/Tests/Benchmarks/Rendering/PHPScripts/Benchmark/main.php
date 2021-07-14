<h2>Set the benchmark</h2>
<form method="get" action="Benchmark/summary.php">
    <label for="fps" >Frames Per Second:</label>
    <input id="aps" name="fps" type="number" required="required" value="1000"/><br/>

    <label for="aps" >Asteroids Per Second:</label>
    <input id="aps" name="aps" type="number" required="required" value="5"/><br/>
    
    <label for="asteroid_size" >Asteroid size:</label>
    <input id="asteroid_size" name="asteroid_size" type="number" required="required" value="20"/><br/>

    <label for="asteroid_speed" >Asteroid speed (Pixels Per Second):</label>
    <input id="asteroid_speed" name="asteroid_speed" type="number" required="required"  value="60"/><br/>

    <label for="background_height" >Background height:</label>
    <input id="background_height" name="background_height" type="number" required="required" value="800"/><br/>

    <label for="background_width" >Background width:</label>
    <input id="background_width" name="background_width" type="number" required="required"  value="800"/><br/>

    <label for="rendering">Rendering:</label>
    <select name="rendering" id="rendering">
        <option value="simple" >String</option>
        <option value="complex" selected>Diff</option>
    </select><br/>

    <label for="duration">Duration:</label>
    <input id="duration" name="duration" type="number" required="required"  value="60"/><br/>

    <input type="submit" value="Submit"/><br/>
</form>