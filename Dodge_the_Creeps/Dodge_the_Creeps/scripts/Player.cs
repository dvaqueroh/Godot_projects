using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed{get;set;} = 400 ; // como de rapido se puede mover el jugador
	[Signal]
	public delegate void HitEventHandler() ; // señar HIT
	public Vector2 ScreenSize ; // tamño de la ventana del juego
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;	
		Hide(); // el jugador comenzara oculto

	}// fin clase _READY


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // Vector de movimiento del jugador
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
			GD.Print("derecha");			
		}	
		if(Input.IsActionPressed("move_left")){
			velocity.X -= 1;
			GD.Print("izquierda");
		}
		if(Input.IsActionPressed("move_up")){
			velocity.Y -= 1;
			GD.Print("arriba");
		}
		if(Input.IsActionPressed("move_down")){
			velocity.Y += 1;
			GD.Print("abajo");
		}
		
		var animatedSprite2D =  GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (velocity.Length() > 0 ){
			velocity = velocity.Normalized() * Speed ;
			animatedSprite2D.Play() ;
		}
		else{
			animatedSprite2D.Stop();
		}

		// asigna movimiento
		Position += velocity * (float)delta ;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)

		);

		// rotacion de la animacion y el Sprite
		if (velocity.X !=0){
			animatedSprite2D.Animation = "walk" ;
			animatedSprite2D.FlipV = false ;
			animatedSprite2D.FlipH = velocity.X < 0 ; 

		} else if (velocity.Y != 0){
			animatedSprite2D.Animation = "up" ;
			animatedSprite2D.FlipV = velocity.Y > 0;
			animatedSprite2D.FlipH = false;
		};
		
	}// fin clase _Process

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("** JUGADOR TOCADO ***") ;
		Hide(); // el jugador desaparece al empezar hit
		EmitSignal(SignalName.Hit);
		// Debe posponerse ya que no podemos cambiar las propiedades físicas en una devolución de llamada de física.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		
	}// fin OnBodyEntered

	public void Start(Vector2 position){
		Position = position; 
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}// fin Star

} // fin class player
