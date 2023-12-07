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
	Vector3 coordenadasIniciales;
	private AnimationPlayer animationPlayer;
	private CsgCombiner3D modeloJugador;
	// Funciones
	private void areaCamaraEntrar(Area3D area){
		var camara = GetNode<Camera3D>("camara");
		RemoveChild(camara);
		camara.Position = Position + new Vector3(0,0.75f,2.45f);
		GetParent().AddChild(camara);
	}

	private void areaCamaraSalir(Area3D area){
		var camara = GetParent().GetNode<Camera3D>("camara");
		GetParent().RemoveChild(camara);
		camara.Position = new Vector3(0,0.75f,2.45f);
		AddChild(camara);
	}
	// Funciones Override
	public override void _Ready()
	{
		animationPlayer =  GetNode<AnimationPlayer>("animaciones");
		modeloJugador = GetNode<CsgCombiner3D>("modelo");
		Area3D areaCamara = GetParent().GetNode<Area3D>("area_camara");

		areaCamara.AreaEntered += areaCamaraEntrar;
		areaCamara.AreaExited += areaCamaraSalir;
		coordenadasIniciales = Position;
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("in_derecha")){
			rotacion.Y = -0.43633187f;
			if(IsOnFloor()){
				if(Math.Abs(Velocity.X)<=4){
					animationPlayer.Play("correr");
				}else{
					animationPlayer.Play("correrRapido",-1,2);
				}
			}
		}
		else if(Input.IsActionPressed("in_izquierda")){
			rotacion.Y = -2.7052608f;
			if(IsOnFloor()){
				if(Math.Abs(Velocity.X)<=4){
					animationPlayer.Play("correr");
				}else{
					animationPlayer.Play("correrRapido",-1,2);
				}
			}
		}else if(IsOnFloor()){
			animationPlayer.Play("idle");
		}
		
		if(IsOnFloor() && Input.IsActionPressed("in_salto")){
			animationPlayer.Play("saltar");
		}

		if(Velocity.Y < -7 && Velocity.Y > -8){
			animationPlayer.Play("caer",-1,2);
		}

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

		if(Position.Y < -10f){
			Position = coordenadasIniciales;
		}

		if(Position.Z != coordenadasIniciales.Z){
			Position = new Vector3(Position.X,Position.Y,coordenadasIniciales.Z);
		}
    }
}
