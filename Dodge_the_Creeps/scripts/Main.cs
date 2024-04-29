using Godot;
using System;
using System.ComponentModel;

public partial class Main : Node
{
	[Export] 
	public PackedScene MobScene{ get; set;}
	private int _score;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Metodo Ready");	

	}// fin ready

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}// fin process

	public void GameOver()
	{
		
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop(); 
		GD.Print("**** GAME OVER ***") ;

		GetNode<HUD>("HUD").ShowGameOver(); // muestra mensaje GAME OVER desde nodo HUD
        GetNode<Timer>("MessageTimer").Start(); // prueba
	}// fin GameOver

	public void NewGame()
	{
		GD.Print(" NEW GAME") ;
		_score = 0 ;
		var player = GetNode<Player>("Player");
		var starPosition = GetNode<Marker2D>("StartPosition");
		player.Start(starPosition.Position); 

		GetNode<Timer>("StartTimer").Start();

		var hud = GetNode<HUD>("HUD"); // creamos el objeto HUD desde su nodo
		hud.UpdateScore(_score); // llamamos a la funcion UpdateScore
		hud.ShowMessage("Get Ready!"); // llamamos a la funcion ShowMessage

		GetTree().CallGroup("mobs",Node.MethodName.QueueFree); // elimna el grupo Mobs, todos los enemigos
		GD.Print("Se borran enemigos");

	}// fin new game

	private void OnScoreTimerTimeOut(){
		GD.Print("sumamos Puntuacion") ;
		_score ++ ;
		// llamamos a la funcion UpdateScore del nodo HUD pasandole la variable _score
		GetNode<HUD>("HUD").UpdateScore(_score); 
	}// final on score time

	private void OnStartTimerOut(){
		//GD.Print("METODO ON START TIME OUT") ;
		OnMobTimerTimeOut();
		GetNode<Timer>("MobTimer").Start() ;
		GetNode<Timer>("ScoreTimer").Start() ;
	}// fin star timer

	private void OnMobTimerTimeOut(){
		// Note: Normally it is best to use explicit types rather than the `var`
		// keyword. However, var is acceptable to use here because the types are
		// obviously Mob and PathFollow2D, since they appear later on the line.

		//GD.Print("METODO ON MOVE TIMER TIME OUT") ;
		// Create a new instance of the Mob scene.
		Mob mob = MobScene.Instantiate<Mob>();

		// Choose a random location on Path2D.
		var MobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		MobSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's direction perpendicular to the path direction.
		float direction = MobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob's position to a random location.
		mob.Position = MobSpawnLocation.Position;
		
		// Add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi /4,Mathf.Pi / 4);
		mob.Rotation = direction ;

		// Choose the velocity.
		var velocity = new Vector2((float)GD.RandRange(150.0,250.0),0);
		mob.LinearVelocity = velocity.Rotated(direction) ;

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob); 
	}// movimiento enemigos

}// fin Clase Main
