using Godot;
using System;
using System.Threading;
using Timer = Godot.Timer;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}// fin Ready

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}// fin _Process

	public void ShowMessage(String texto)
	{
		var message = GetNode<Label>("Message"); // creamos el objeto message trayendo el nodo label "message"
		message.Text = texto; // pasamos el valos del texto
		message.Show(); // muestra el mensaje
		GD.Print("NODO ShowMessage");
		GetNode<Timer>("MessageTimer").Start();
		
	} // fin showMessage

	async public void ShowGameOver()
    {
        GD.Print("METODO SHOW GAME OVER");
        ShowMessage("Game Over"); // llamamos a la funcion mostrar mensaje

        GD.Print("NODO TIMER MESSAGETIMER");
        var messageTimer = GetNode<Timer>("MessageTimer"); // instanciamos el nodo Timer MessageTimer
        await ToSignal(messageTimer, Timer.SignalName.Timeout);

        var message = GetNode<Label>("Message"); // instanciamos el nodo Label message
        message.Text = "Dodge the creeps!";
        message.Show();

        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        GetNode<Button>("StartButton").Show();

    }// fin show game over

   
    public void UpdateScore(int Score)
	{
		GetNode<Label>("ScoreLabel").Text =  Score.ToString();
	} // fin UpdateScore

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide(); // instancia el nodo button, lo oculta
		EmitSignal(SignalName.StartGame);
		GD.Print("BOTON START PULSADO") ;
		OnMessageTimerTimeOut();
	} // fin OnStartBorronPressed

	private void OnMessageTimerTimeOut()
	{
		GetNode<Label>("Message").Hide(); // oculta el nodo label
	}

}// fin clase
