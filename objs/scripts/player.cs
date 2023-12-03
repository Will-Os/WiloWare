using Godot;
using System;

public partial class player : CharacterBody3D
{
	//Variables base
	float velocidad = 3f;
	const float salto = 1f;
	const float gravedad = 15f;
	Vector3 movimiento = new Vector3();
	Vector3 rotacion = new Vector3(0,-0.43633187f,0);
	private AnimationPlayer animationPlayer;
	private CsgCombiner3D modeloJugador;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//animationPlayer =  GetNode<AnimationPlayer>("animaciones");
		modeloJugador = GetNode<CsgCombiner3D>("modelo");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("in_derecha")){
			rotacion.Y = -0.43633187f;
		}
		if(Input.IsActionPressed("in_izquierda")){
			rotacion.Y = -2.7052608f;
		}
		/*if(movimiento.X != 0){
			animationPlayer.Play("correr");
		}else{
			animationPlayer.Play("idle");
		}*/
		modeloJugador.Rotation= rotacion;
	}

    public override void _PhysicsProcess(double delta)
    {
		movimiento.X = 0;
		movimiento.Y -= gravedad*(float)delta;
		float velocidad = 4f;
		if(Input.IsActionPressed("in_sprint")){
			velocidad += 1.5f;
		}

		if(Input.IsActionPressed("in_derecha")){
			movimiento.X += velocidad;
		}

		if(Input.IsActionPressed("in_izquierda")){
			movimiento.X -= velocidad;
		}
		
		Velocity = movimiento;
		MoveAndSlide();

		
		if(IsOnFloor()){
			movimiento.Y = 0;

			if(Input.IsActionPressed("in_salto")){
				movimiento.Y = (float)Math.Sqrt(2*gravedad*salto);
			}
		}
    }
}
