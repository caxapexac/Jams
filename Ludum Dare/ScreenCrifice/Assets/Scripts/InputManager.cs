using LeopotamGroup.Math;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[HideInInspector]
	public Man Man;

	private bool _isWalk;
	
	void Start ()
	{
		Man = GetComponent<Man>();
	}

	void Update ()
	{
		#region Debug

		if (Input.GetAxis("Fix") > 0)
		{
			//Man.DeathM.FixPixel();
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			//Man.Alive = !Man.Alive;
		}

		#endregion
		
		
		Man.PlayerM.Force.x = Input.GetAxis("Horizontal");
		Man.PlayerM.Force.y = Input.GetAxis("Vertical");
		_isWalk = Man.PlayerM.Force.sqrMagnitude > 0.01f;
		Man.PlayerM.Animator.SetBool("Walk", _isWalk);
		Man.PlayerM.Force = Vector2.ClampMagnitude(Man.PlayerM.Force, 1);
		if (Man.SlowedDown) Man.PlayerM.Force *= Man.SlowMultipiler;
		Vector2 moveVector = (Vector2) Man.PlayerM.Player.localPosition + Man.PlayerM.Force * Man.SpeedMultipiler * Time.deltaTime;
		Man.PlayerM.Player.GetComponent<Rigidbody2D>().MovePosition(moveVector);
		Man.PlayerM.PlayerSprite.LookAt2D(!_isWalk ? Man.MousePos : moveVector);
		Man.PlayerM.PlayerHead.LookAt2D(Man.MousePos);
	}
}
